using NightMarket.Admin.Commons;
using NightMarket.Admin.ProductAttributes;
using NightMarket.Admin.Products.Attributes;
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
            PagedResultRequestDto,
            CreateUpdateProductDto>
    {
        Task<PagedResultDto<ProductInListDto>> GetListWithFilterAsync(ProductListFilterDto input);


        Task<List<ProductInListDto>> GetListAllAsync();

        Task DeleteMultipleAsync(IEnumerable<Guid> ids);

        Task<string> GetThumbnailImageAsync(string fileName);

        Task<string> GetSuggestNewCodeAsync();

        Task<ProductAttributeValueDto> AddProductAttributeAsync(AddUpdateProductAttributeDto input);
		Task<ProductAttributeValueDto> UpdateProductAttributeAsync(Guid id,AddUpdateProductAttributeDto input);


		Task RemoveProductAttributeAsync(Guid attributeId, Guid id);

        Task<List<ProductAttributeValueDto>> GetListProductAttributeAllAsync(Guid productId);

		Task<PagedResultDto<ProductAttributeValueDto>> GetListProductAttributeAsync(ProductAttributeListFilterDto input);

	}
}
