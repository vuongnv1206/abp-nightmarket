using AutoMapper;
using NightMarket.Admin.ProductCategories;
using NightMarket.ProductCategories;

namespace NightMarket.Admin;

public class NightMarketAdminApplicationAutoMapperProfile : Profile
{
    public NightMarketAdminApplicationAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */

        CreateMap<ProductCategory,ProductCategoryDto>();
        CreateMap<ProductCategory, ProductCategoryInListDto>();
        CreateMap<ProductCategory, CreateUpdateProductCategoryDto>();


    }
}
