using System.Threading.Tasks;

namespace NightMarket.Data;

public interface INightMarketDbSchemaMigrator
{
    Task MigrateAsync();
}
