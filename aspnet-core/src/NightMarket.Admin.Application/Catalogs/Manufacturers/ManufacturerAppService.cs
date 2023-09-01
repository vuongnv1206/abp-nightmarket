using Microsoft.AspNetCore.Authorization;
using NightMarket.Admin.Commons;
using NightMarket.Admin.Permissions;
using NightMarket.Catalogs.Manufacturers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;

namespace NightMarket.Admin.Catalogs.Manufacturers
{
    [Authorize(NightMarketPermissions.Manufacturer.Default,Policy ="AdminOnly")]
    public class ManufacturerAppService :
        CrudAppService<
            Manufacturer,
            ManufacturerDto,
            Guid,
            PagedResultRequestDto,
            CreateUpdateManufacturerDto>,
        IManufacturerAppService
    {
        public ManufacturerAppService(IRepository<Manufacturer, Guid> repository) : base(repository)
        {
			GetPolicyName = NightMarketPermissions.Manufacturer.Default;
			GetListPolicyName = NightMarketPermissions.Manufacturer.Default;
			CreatePolicyName = NightMarketPermissions.Manufacturer.Create;
			UpdatePolicyName = NightMarketPermissions.Manufacturer.Update;
			DeletePolicyName = NightMarketPermissions.Manufacturer.Delete;
		}

        [Authorize(NightMarketPermissions.Manufacturer.Delete)]
        public async Task DeleteMultipleAsync(IEnumerable<Guid> ids)
        {
            await Repository.DeleteManyAsync(ids);
            await UnitOfWorkManager.Current.SaveChangesAsync();
        }

		[Authorize(NightMarketPermissions.Manufacturer.Default)]

		public async Task<List<ManufacturerInListDto>> GetListAllAsync()
        {
            var query = await Repository.GetQueryableAsync();

            query = query.Where(x => x.IsActive == true);

            var data = await AsyncExecuter.ToListAsync(query);

            return ObjectMapper.Map<List<Manufacturer>, List<ManufacturerInListDto>>(data);
        }
		[Authorize(NightMarketPermissions.Manufacturer.Default)]


		public async Task<PagedResultDto<ManufacturerInListDto>> GetListWithFilterAsync(BaseListFilterDto input)
        {
            var query = await Repository.GetQueryableAsync();
            query = query.WhereIf(!string.IsNullOrEmpty(input.KeyWord), x => x.Name.Contains(input.KeyWord));

            var totalCount = await AsyncExecuter.LongCountAsync(query);

            var data = await AsyncExecuter.ToListAsync(query.OrderByDescending(x => x.CreationTime).Skip(input.SkipCount).Take(input.MaxResultCount));

            return new PagedResultDto<ManufacturerInListDto>()
            {
                TotalCount = totalCount,
                Items = ObjectMapper.Map<List<Manufacturer>, List<ManufacturerInListDto>>(data)
            };
        }
    }
}
