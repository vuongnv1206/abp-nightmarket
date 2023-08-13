using NightMarket.Admin.Commons;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NightMarket.Admin.ProductCategories
{
    public interface IProductCategoryAppService : 
        ICrudAppService<ProductCategoryDto,
            Guid,
            PagedAndSortedResultRequestDto,
            CreateUpdateProductCategoryDto>
    {

        Task<PagedResultDto<ProductCategoryInListDto>> GetListWithFilterAsync(BaseListFilterDto input);
    
        
        Task<List<ProductCategoryInListDto>> GetListAllAsync();

        Task DeleteMultipleAsync(IEnumerable<Guid> ids);

    }
}
