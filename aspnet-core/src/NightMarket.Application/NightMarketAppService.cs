using System;
using System.Collections.Generic;
using System.Text;
using NightMarket.Localization;
using Volo.Abp.Application.Services;

namespace NightMarket;

/* Inherit your application services from this class.
 */
public abstract class NightMarketAppService : ApplicationService
{
    protected NightMarketAppService()
    {
        LocalizationResource = typeof(NightMarketResource);
    }
}
