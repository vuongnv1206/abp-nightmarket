using Volo.Abp.Ui.Branding;
using Volo.Abp.DependencyInjection;

namespace NightMarket;

[Dependency(ReplaceServices = true)]
public class NightMarketBrandingProvider : DefaultBrandingProvider
{
    public override string AppName => "NightMarket";
}
