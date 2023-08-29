using AutoMapper;
using NightMarket.Admin.Manufacturers;
using NightMarket.Admin.ProductAttributes;
using NightMarket.Admin.ProductCategories;
using NightMarket.Admin.Products;
using NightMarket.Admin.Roles;
using NightMarket.Manufacturers;
using NightMarket.ProductAttributes;
using NightMarket.ProductCategories;
using NightMarket.Products;
using NightMarket.Roles;
using Volo.Abp.Identity;

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

		//ProductAttribute
		CreateMap<ProductAttribute, ProductAttributeDto>().ReverseMap();
		CreateMap<ProductAttribute, ProductAttributeInListDto>().ReverseMap();
		CreateMap<ProductAttribute, CreateUpdateProductAttributeDto>().ReverseMap();

		//Roles
		CreateMap<IdentityRole, RoleDto>().ForMember(x => x.Description,
			map => map.MapFrom(x => x.ExtraProperties.ContainsKey(RoleConsts.DescriptionFieldName)
			?
			x.ExtraProperties[RoleConsts.DescriptionFieldName]  :null));
		CreateMap<IdentityRole, RoleInListDto>().ForMember(x => x.Description,
			map => map.MapFrom(x => x.ExtraProperties.ContainsKey(RoleConsts.DescriptionFieldName)
			? x.ExtraProperties[RoleConsts.DescriptionFieldName] : null));
		CreateMap<CreateUpdateRoleDto, IdentityRole>();
	}
}
