using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Shared.Data.Seed;

namespace Shared.Data;

public static class Extentions
{
    public static IApplicationBuilder UseMigration<TContext>(this IApplicationBuilder app)
        where TContext : DbContext
    {

        MigrateDatabaseAsync<TContext>(app.ApplicationServices).GetAwaiter().GetResult();
        SeedDataAnsync(app.ApplicationServices).GetAwaiter().GetResult();

        return app;
    }



    private static async Task MigrateDatabaseAsync<TContext>(IServiceProvider serviceProvider)
       where TContext : DbContext
    {
        using var scope = serviceProvider.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<TContext>();
        await context.Database.MigrateAsync();
    }    
    
    private static async Task SeedDataAnsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var seedeers = scope.ServiceProvider.GetServices<IDataSeeder>();
        foreach (var seeder in seedeers)
        {
            await seeder.SeedAllAsync();
        }

    }
}
