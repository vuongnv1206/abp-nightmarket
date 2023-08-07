using Volo.Abp.Modularity;

namespace NightMarket;

[DependsOn(
    typeof(NightMarketApplicationModule),
    typeof(NightMarketDomainTestModule)
    )]
public class NightMarketApplicationTestModule : AbpModule
{

}
