using AutoMapper;
using NightMarket.Admin.ProductCategories;
using NightMarket.Admin.Products;
using NightMarket.ProductCategories;
using NightMarket.Products;

namespace NightMarket.Admin;

public class NightMarketAdminApplicationAutoMapperProfile : Profile
{
    public NightMarketAdminApplicationAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */

        //Product-Category
        CreateMap<ProductCategory,ProductCategoryDto>();
        CreateMap<ProductCategory, ProductCategoryInListDto>();
        CreateMap<ProductCategory, CreateUpdateProductCategoryDto>();


        //Product
        CreateMap<Product, ProductDto>();
        CreateMap<Product, ProductInListDto>();
        CreateMap<Product, CreateUpdateProductDto>();

    }
}
