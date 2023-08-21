using NightMarket.Admin.Commons;
using NightMarket.ProductAttributes;
using NightMarket.ProductCategories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;

namespace NightMarket.Admin.ProductAttributes
{
	public class ProductAttributeAppService :
		CrudAppService<
			ProductAttribute,
			ProductAttributeDto,
			Guid,
			PagedAndSortedResultRequestDto,
			CreateUpdateProductAttributeDto>,
		IProductAttributeAppService
	{
		public ProductAttributeAppService(IRepository<ProductAttribute, Guid> repository) : base(repository)
		{
		}

		public async Task DeleteMultipleAsync(IEnumerable<Guid> ids)
		{
			await Repository.DeleteManyAsync(ids);
			await UnitOfWorkManager.Current.SaveChangesAsync();
		}

		public async Task<List<ProductAttributeInListDto>> GetListAllAsync()
		{
			var query = await Repository.GetQueryableAsync();

			query = query.Where(x => x.IsActive == true);

			var data = await AsyncExecuter.ToListAsync(query);

			return ObjectMapper.Map<List<ProductAttribute>, List<ProductAttributeInListDto>>(data);
		}

		public async Task<PagedResultDto<ProductAttributeInListDto>> GetListWithFilterAsync(BaseListFilterDto input)
		{
			var query = await Repository.GetQueryableAsync();
			query = query.WhereIf(!string.IsNullOrEmpty(input.KeyWord), x => x.Label.Contains(input.KeyWord));

			var totalCount = await AsyncExecuter.LongCountAsync(query);

			var data = await AsyncExecuter.ToListAsync(query.OrderByDescending(x => x.CreationTime).Skip(input.SkipCount).Take(input.MaxResultCount));

			return new PagedResultDto<ProductAttributeInListDto>()
			{
				TotalCount = totalCount,
				Items = ObjectMapper.Map<List<ProductAttribute>, List<ProductAttributeInListDto>>(data)
			};
		}
	}
}
