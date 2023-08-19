using Volo.Abp.Account;
using Volo.Abp.AutoMapper;
using Volo.Abp.BlobStoring;
using Volo.Abp.BlobStoring.FileSystem;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement;
using Volo.Abp.SettingManagement;
using Volo.Abp.TenantManagement;

namespace NightMarket.Admin;

[DependsOn(
    typeof(NightMarketDomainModule),
    typeof(AbpAccountApplicationModule),
    typeof(NightMarketAdminApplicationContractsModule),
    typeof(AbpIdentityApplicationModule),
    typeof(AbpPermissionManagementApplicationModule),
    typeof(AbpTenantManagementApplicationModule),
    typeof(AbpFeatureManagementApplicationModule),
    typeof(AbpSettingManagementApplicationModule),
    typeof(AbpBlobStoringFileSystemModule)
    )]
public class NightMarketAdminApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<NightMarketAdminApplicationModule>();
        });

		//Blob storage
		Configure<AbpBlobStoringOptions>(options =>
		{
			options.Containers.Configure("product-thumbnail-pictures", container =>
			{
                //TODO...
                container.UseFileSystem(fileSystem =>
                {
                    fileSystem.BasePath = "D:\\nightmarket";
                });
			});
		});

	}
}
