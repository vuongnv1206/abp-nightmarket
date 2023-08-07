using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace NightMarket.Data;

/* This is used if database provider does't define
 * INightMarketDbSchemaMigrator implementation.
 */
public class NullNightMarketDbSchemaMigrator : INightMarketDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
