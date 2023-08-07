using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NightMarket.Data;
using Volo.Abp.DependencyInjection;

namespace NightMarket.EntityFrameworkCore;

public class EntityFrameworkCoreNightMarketDbSchemaMigrator
    : INightMarketDbSchemaMigrator, ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;

    public EntityFrameworkCoreNightMarketDbSchemaMigrator(
        IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task MigrateAsync()
    {
        /* We intentionally resolving the NightMarketDbContext
         * from IServiceProvider (instead of directly injecting it)
         * to properly get the connection string of the current tenant in the
         * current scope.
         */

        await _serviceProvider
            .GetRequiredService<NightMarketDbContext>()
            .Database
            .MigrateAsync();
    }
}
