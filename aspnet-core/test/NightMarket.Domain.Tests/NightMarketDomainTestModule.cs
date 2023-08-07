using NightMarket.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace NightMarket;

[DependsOn(
    typeof(NightMarketEntityFrameworkCoreTestModule)
    )]
public class NightMarketDomainTestModule : AbpModule
{

}
