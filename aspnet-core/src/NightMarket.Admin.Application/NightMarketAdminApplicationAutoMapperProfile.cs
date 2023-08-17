using AutoMapper;
using NightMarket.Admin.Manufacturers;
using NightMarket.Admin.ProductCategories;
using NightMarket.Admin.Products;
using NightMarket.Manufacturers;
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
        CreateMap<ProductCategory,ProductCategoryDto>().ReverseMap();
        CreateMap<ProductCategory, ProductCategoryInListDto>().ReverseMap();
        CreateMap<ProductCategory, CreateUpdateProductCategoryDto>().ReverseMap();


        //Product
        CreateMap<Product, ProductDto>().ReverseMap();
        CreateMap<Product, ProductInListDto>().ReverseMap();
        CreateMap<Product, CreateUpdateProductDto>().ReverseMap();

        //Manufacturer
        CreateMap<Manufacturer, ManufacturerDto>().ReverseMap();
        CreateMap<Manufacturer, ManufacturerInListDto>().ReverseMap();
        CreateMap<Manufacturer, CreateUpdateManufacturerDto>().ReverseMap();

    }
}
