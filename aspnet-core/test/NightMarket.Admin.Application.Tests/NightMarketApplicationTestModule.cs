using Volo.Abp.Modularity;

namespace NightMarket.Admin;

[DependsOn(
    typeof(NightMarketAdminApplicationModule),
    typeof(NightMarketDomainTestModule)
    )]
public class NightMarketApplicationTestModule : AbpModule
{

}
