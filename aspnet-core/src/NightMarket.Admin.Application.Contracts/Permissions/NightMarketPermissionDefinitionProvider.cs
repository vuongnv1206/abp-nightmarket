using NightMarket.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace NightMarket.Admin.Permissions;

public class NightMarketPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {

		//Define your own permissions here. Example:
		//myGroup.AddPermission(NightMarketPermissions.MyPermission1, L("Permission:MyPermission1"));

		//Catalog Group
		var catalogGroup = context.AddGroup(NightMarketPermissions.CatalogGroupName, L("Permission:Catalog"));

		//Manufacture
		var manufacturerPermission = catalogGroup.AddPermission(NightMarketPermissions.Manufacturer.Default, L("Permission:Catalog.Manufacturer"));
		manufacturerPermission.AddChild(NightMarketPermissions.Manufacturer.Create, L("Permission:Catalog.Manufacturer.Create"));
		manufacturerPermission.AddChild(NightMarketPermissions.Manufacturer.Update, L("Permission:Catalog.Manufacturer.Update"));
		manufacturerPermission.AddChild(NightMarketPermissions.Manufacturer.Delete, L("Permission:Catalog.Manufacturer.Delete"));

		//Product Category
		var productCategoryPermission = catalogGroup.AddPermission(NightMarketPermissions.ProductCategory.Default, L("Permission:Catalog.ProductCategory"));
		productCategoryPermission.AddChild(NightMarketPermissions.ProductCategory.Create, L("Permission:Catalog.ProductCategory.Create"));
		productCategoryPermission.AddChild(NightMarketPermissions.ProductCategory.Update, L("Permission:Catalog.ProductCategory.Update"));
		productCategoryPermission.AddChild(NightMarketPermissions.ProductCategory.Delete, L("Permission:Catalog.ProductCategory.Delete"));

		//Add product
		var productPermission = catalogGroup.AddPermission(NightMarketPermissions.Product.Default, L("Permission:Catalog.Product"));
		productPermission.AddChild(NightMarketPermissions.Product.Create, L("Permission:Catalog.Product.Create"));
		productPermission.AddChild(NightMarketPermissions.Product.Update, L("Permission:Catalog.Product.Update"));
		productPermission.AddChild(NightMarketPermissions.Product.Delete, L("Permission:Catalog.Product.Delete"));
		productPermission.AddChild(NightMarketPermissions.Product.AttributeManage, L("Permission:Catalog.Product.AttributeManage"));

		//Add attribute
		var attributePermission = catalogGroup.AddPermission(NightMarketPermissions.Attribute.Default, L("Permission:Catalog.Attribute"));
		attributePermission.AddChild(NightMarketPermissions.Attribute.Create, L("Permission:Catalog.Attribute.Create"));
		attributePermission.AddChild(NightMarketPermissions.Attribute.Update, L("Permission:Catalog.Attribute.Update"));
		attributePermission.AddChild(NightMarketPermissions.Attribute.Delete, L("Permission:Catalog.Attribute.Delete"));

	}

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<NightMarketResource>(name);
    }
}
