using NightMarket.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace NightMarket.Admin.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class NightMarketController : AbpControllerBase
{
    protected NightMarketController()
    {
        LocalizationResource = typeof(NightMarketResource);
    }
}
