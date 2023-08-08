using System;
using System.Collections.Generic;
using System.Text;
using NightMarket.Localization;
using Volo.Abp.Application.Services;

namespace NightMarket.Admin;

/* Inherit your application services from this class.
 */
public abstract class NightMarketAdminAppService : ApplicationService
{
    protected NightMarketAdminAppService()
    {
        LocalizationResource = typeof(NightMarketResource);
    }
}
