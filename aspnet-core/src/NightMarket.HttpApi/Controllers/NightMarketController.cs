using NightMarket.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace NightMarket.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class NightMarketController : AbpControllerBase
{
    protected NightMarketController()
    {
        LocalizationResource = typeof(NightMarketResource);
    }
}
