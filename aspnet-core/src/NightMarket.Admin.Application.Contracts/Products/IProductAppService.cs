using NightMarket.Admin.Commons;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NightMarket.Admin.Products
{
    public interface IProductAppService : 
        ICrudAppService<ProductDto,
            Guid,
            PagedAndSortedResultRequestDto,
            CreateUpdateProductDto>
    {
        Task<PagedResultDto<ProductInListDto>> GetListWithFilterAsync(ProductListFilterDto input);


        Task<List<ProductInListDto>> GetListAllAsync();

        Task DeleteMultipleAsync(IEnumerable<Guid> ids);
    }
}
