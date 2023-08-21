using NightMarket.Admin.Commons;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NightMarket.Admin.ProductAttributes
{
	public interface IProductAttributeAppService :
	ICrudAppService<ProductAttributeDto,
			Guid,
			PagedAndSortedResultRequestDto,
			CreateUpdateProductAttributeDto>
	{

		Task<PagedResultDto<ProductAttributeInListDto>> GetListWithFilterAsync(BaseListFilterDto input);


		Task<List<ProductAttributeInListDto>> GetListAllAsync();

		Task DeleteMultipleAsync(IEnumerable<Guid> ids);

	}
}
