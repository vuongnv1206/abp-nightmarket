using Microsoft.AspNetCore.Authorization;
using NightMarket.Admin.Commons;
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
using Volo.Abp.PermissionManagement;
using Volo.Abp.SimpleStateChecking;

namespace NightMarket.Admin.Roles
{
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
		
		public RoleAppService(IRepository<IdentityRole, Guid> repository) : base(repository)
		{
		}

		public async Task DeleteMultipleAsync(IEnumerable<Guid> ids)
		{
			await Repository.DeleteManyAsync(ids);
			await UnitOfWorkManager.Current.SaveChangesAsync();
		}

		public async Task<List<RoleInListDto>> GetListAllAsync()
		{
			var query = await Repository.GetQueryableAsync();
			var data = await AsyncExecuter.ToListAsync(query);

			return ObjectMapper.Map<List<IdentityRole>, List<RoleInListDto>>(data);

		}

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

		public async Task<GetPermissionListResultDto> GetPermissionsAsync(string providerName, string providerKey)
		{
			throw new NotImplementedException();
		}

		public async Task UpdatePermissionsAsync(string providerName, string providerKey, UpdatePermissionsDto input)
		{
			throw new NotImplementedException();
		}
	}
}
