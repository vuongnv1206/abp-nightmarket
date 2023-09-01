using AutoMapper;
using NightMarket.Admin.Catalogs.Manufacturers;
using NightMarket.Admin.Catalogs.ProductAttributes;
using NightMarket.Admin.Catalogs.ProductCategories;
using NightMarket.Admin.Catalogs.Products;
using NightMarket.Admin.Systems.Roles;
using NightMarket.Admin.Systems.Users;
using NightMarket.Catalogs.Manufacturers;
using NightMarket.Catalogs.ProductAttributes;
using NightMarket.Catalogs.ProductCategories;
using NightMarket.Catalogs.Products;
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

		//User
		CreateMap<IdentityUser, UserDto>().ReverseMap();
		CreateMap<IdentityUser, UserInListDto>().ReverseMap();

	}
}
