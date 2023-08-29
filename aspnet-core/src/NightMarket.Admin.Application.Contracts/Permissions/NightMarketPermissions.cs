﻿namespace NightMarket.Admin.Permissions;

public static class NightMarketPermissions
{


	//Add your own permission names. Example:
	//public const string MyPermission1 = GroupName + ".MyPermission1";

	
	public const string CatalogGroupName = "NightMarketAdminCatalog";


	public static class Product
	{
		public const string Default = CatalogGroupName + ".Product";
		public const string Create = Default + ".Create";
		public const string Update = Default + ".Update";
		public const string Delete = Default + ".Delete";
		public const string AttributeManage = Default + ".Attribute";

	}

	public static class Attribute
	{
		public const string Default = CatalogGroupName + ".Attribute";
		public const string Create = Default + ".Create";
		public const string Update = Default + ".Update";
		public const string Delete = Default + ".Delete";
	}

	public static class Manufacturer
	{
		public const string Default = CatalogGroupName + ".Manufacturer";
		public const string Create = Default + ".Create";
		public const string Update = Default + ".Update";
		public const string Delete = Default + ".Delete";
	}
	public static class ProductCategory
	{
		public const string Default = CatalogGroupName + ".ProductCategory";
		public const string Create = Default + ".Create";
		public const string Update = Default + ".Update";
		public const string Delete = Default + ".Delete";
	}
}
