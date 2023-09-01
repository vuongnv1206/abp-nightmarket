﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using NightMarket.Admin.Commons;
using NightMarket.Admin.Permissions;
using NightMarket.Roles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;
using Volo.Abp.OpenIddict;
using Volo.Abp.PermissionManagement;
using Volo.Abp.SimpleStateChecking;

namespace NightMarket.Admin.Systems.Roles
{
	[Authorize(IdentityPermissions.Roles.Default, Policy = "AdminOnly")]

	public class RoleAppService : CrudAppService<
        IdentityRole,
        RoleDto,
        Guid,
        PagedResultRequestDto,
        CreateUpdateRoleDto,
        CreateUpdateRoleDto>, IRoleAppService
    {
        protected PermissionManagementOptions Options { get; }
        protected IPermissionManager PermissionManager { get; }
        protected IPermissionDefinitionManager PermissionDefinitionManager { get; }
        protected ISimpleStateCheckerManager<PermissionDefinition> SimpleStateCheckerManager { get; }

        public RoleAppService(IRepository<IdentityRole, Guid> repository,
        IPermissionManager permissionManager,
        IPermissionDefinitionManager permissionDefinitionManager,
        IOptions<PermissionManagementOptions> options,
        ISimpleStateCheckerManager<PermissionDefinition> simpleStateCheckerManager)
            : base(repository)
        {
            Options = options.Value;
            PermissionManager = permissionManager;
            PermissionDefinitionManager = permissionDefinitionManager;
            SimpleStateCheckerManager = simpleStateCheckerManager;

			GetPolicyName = IdentityPermissions.Roles.Default;
			GetListPolicyName = IdentityPermissions.Roles.Default;
			CreatePolicyName = IdentityPermissions.Roles.Create;
			UpdatePolicyName = IdentityPermissions.Roles.Update;
			DeletePolicyName = IdentityPermissions.Roles.Delete;
		}

		[Authorize(IdentityPermissions.Roles.Delete)]
		public async Task DeleteMultipleAsync(IEnumerable<Guid> ids)
        {
            await Repository.DeleteManyAsync(ids);
            await UnitOfWorkManager.Current.SaveChangesAsync();
        }
		[Authorize(IdentityPermissions.Roles.Default)]
		public async Task<List<RoleInListDto>> GetListAllAsync()
        {
            var query = await Repository.GetQueryableAsync();
            var data = await AsyncExecuter.ToListAsync(query);

            return ObjectMapper.Map<List<IdentityRole>, List<RoleInListDto>>(data);

        }
		[Authorize(IdentityPermissions.Roles.Default)]
		public async Task<PagedResultDto<RoleInListDto>> GetListFilterAsync(BaseListFilterDto input)
        {
            var query = await Repository.GetQueryableAsync();
            query = query.WhereIf(!string.IsNullOrWhiteSpace(input.KeyWord), x => x.Name.Contains(input.KeyWord));

            var totalCount = await AsyncExecuter.LongCountAsync(query);
            var data = await AsyncExecuter.ToListAsync(query.Skip(input.SkipCount).Take(input.MaxResultCount));

            return new PagedResultDto<RoleInListDto>(totalCount, ObjectMapper.Map<List<IdentityRole>, List<RoleInListDto>>(data));
        }




		[Authorize(IdentityPermissions.Roles.Create)]
		public async override Task<RoleDto> CreateAsync(CreateUpdateRoleDto input)
        {
            var query = await Repository.GetQueryableAsync();
            var isNameExisted = query.Any(x => x.Name == input.Name);
            if (isNameExisted)
            {
                throw new BusinessException(NightMarketDomainErrorCodes.RoleNameAlreadyExists, $"Role name is already existed {input.Name}").WithData("Name", input.Name);
            }
            var role = new IdentityRole(GuidGenerator.Create(), input.Name);
            role.ExtraProperties[RoleConsts.DescriptionFieldName] = input.Description;
            var data = await Repository.InsertAsync(role);
            await UnitOfWorkManager.Current.SaveChangesAsync();
            return ObjectMapper.Map<IdentityRole, RoleDto>(data);
        }


		[Authorize(IdentityPermissions.Roles.Update)]
		public async override Task<RoleDto> UpdateAsync(Guid id, CreateUpdateRoleDto input)
        {
            var role = await Repository.GetAsync(id);
            if (role == null)
            {
                throw new EntityNotFoundException(typeof(IdentityRole), id);
            }
            var query = await Repository.GetQueryableAsync();
            var isNameExisted = query.Any(x => x.Name == input.Name && x.Id != id);
            if (isNameExisted)
            {
                throw new BusinessException(NightMarketDomainErrorCodes.RoleNameAlreadyExists, $"Role name is already existed {input.Name}").WithData("Name", input.Name);

            }
            role.ExtraProperties[RoleConsts.DescriptionFieldName] = input.Description;
            var data = await Repository.UpdateAsync(role);
            await UnitOfWorkManager.Current.SaveChangesAsync();
            return ObjectMapper.Map<IdentityRole, RoleDto>(data);
        }
		[Authorize(IdentityPermissions.Roles.Default)]
		public async Task<GetPermissionListResultDto> GetPermissionsAsync(string providerName, string providerKey)
        {
            //await CheckProviderPolicy(providerName);

            var result = new GetPermissionListResultDto
            {
                EntityDisplayName = providerKey,
                Groups = new List<PermissionGroupDto>()
            };

            //var multiTenancySide = CurrentTenant.GetMultiTenancySide();



            foreach (var group in await PermissionDefinitionManager.GetGroupsAsync())
            {
                var groupDto = CreatePermissionGroupDto(group);

                var neededCheckPermissions = new List<PermissionDefinition>();

                var permissions = group.GetPermissionsWithChildren()
                    .Where(x => x.IsEnabled)
                    .Where(x => !x.Providers.Any() || x.Providers.Contains(providerName));
                //.Where(x => x.MultiTenancySide.HasFlag(multiTenancySide));

                foreach (var permission in permissions)
                {
                    if (await SimpleStateCheckerManager.IsEnabledAsync(permission))
                    {
                        neededCheckPermissions.Add(permission);
                    }
                }

                if (!neededCheckPermissions.Any())
                {
                    continue;
                }

                var grantInfoDtos = neededCheckPermissions
                    .Select(CreatePermissionGrantInfoDto)
                    .ToList();

                var multipleGrantInfo = await PermissionManager.GetAsync(neededCheckPermissions.Select(x => x.Name).ToArray(), providerName, providerKey);

                foreach (var grantInfo in multipleGrantInfo.Result)
                {
                    var grantInfoDto = grantInfoDtos.First(x => x.Name == grantInfo.Name);

                    grantInfoDto.IsGranted = grantInfo.IsGranted;

                    foreach (var provider in grantInfo.Providers)
                    {
                        grantInfoDto.GrantedProviders.Add(new ProviderInfoDto
                        {
                            ProviderName = provider.Name,
                            ProviderKey = provider.Key,
                        });
                    }

                    groupDto.Permissions.Add(grantInfoDto);
                }

                if (groupDto.Permissions.Any())
                {
                    result.Groups.Add(groupDto);
                }
            }

            return result;
        }
		[Authorize(IdentityPermissions.Roles.Create)]
		private PermissionGrantInfoDto CreatePermissionGrantInfoDto(PermissionDefinition permission)
        {
            return new PermissionGrantInfoDto
            {
                Name = permission.Name,
                DisplayName = permission.DisplayName?.Localize(StringLocalizerFactory),
                ParentName = permission.Parent?.Name,
                AllowedProviders = permission.Providers,
                GrantedProviders = new List<ProviderInfoDto>()
            };
        }
		[Authorize(IdentityPermissions.Roles.Create)]
		private PermissionGroupDto CreatePermissionGroupDto(PermissionGroupDefinition group)
        {
            var localizableDisplayName = group.DisplayName as LocalizableString;

            return new PermissionGroupDto
            {
                Name = group.Name,
                DisplayName = group.DisplayName?.Localize(StringLocalizerFactory),
                DisplayNameKey = localizableDisplayName?.Name,
                DisplayNameResource = localizableDisplayName?.ResourceType != null
                    ? LocalizationResourceNameAttribute.GetName(localizableDisplayName.ResourceType)
                    : null,
                Permissions = new List<PermissionGrantInfoDto>()
            };
        }



        protected virtual async Task CheckProviderPolicy(string providerName)
        {
            var policyName = Options.ProviderPolicies.GetOrDefault(providerName);
            if (policyName.IsNullOrEmpty())
            {
                throw new AbpException($"No policy defined to get/set permissions for the provider '{providerName}'. Use {nameof(PermissionManagementOptions)} to map the policy.");
            }

            await AuthorizationService.CheckAsync(policyName);
        }

        public virtual async Task UpdatePermissionsAsync(string providerName, string providerKey, UpdatePermissionsDto input)
        {
            // await CheckProviderPolicy(providerName);

            foreach (var permissionDto in input.Permissions)
            {
                await PermissionManager.SetAsync(permissionDto.Name, providerName, providerKey, permissionDto.IsGranted);
            }
        }
    }
}
