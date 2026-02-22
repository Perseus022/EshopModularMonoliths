using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Data.Interceptors;
using Shared.Data;

namespace Basket;

public static class BasketModule
{
    public static IServiceCollection AddBasketModule(this IServiceCollection services,
        IConfiguration configuration)
    {
        //Application Use Case Services

        services.AddScoped<IBasketRepository, BasketRepository>();
        services.Decorate<IBasketRepository, CachedBasketRepository>(); //Registration Deccorator for caching with Scrutor

        //Simle Registration Deccorator for caching******

        //services.AddScoped<IBasketRepository>(provider =>
        //{
        //    var basketRepository = provider.GetRequiredService<BasketRepository>();
        //    return new CachedBasketRepository(basketRepository,provider.GetRequiredService<IDistributedCache>());
        //});
        //*******

        //Data Infrastructure Services
        var connectionString = configuration.GetConnectionString("Database");

        services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
        services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterseptor>();

        services.AddDbContext<BasketDbContext>((sp, options) =>
        {
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
            options.UseNpgsql(connectionString);
        });
        //services.AddScoped<IDataSeeder, BasketDataSeeder>();


        return services;
    }

    public static IApplicationBuilder UseBasketModule(this IApplicationBuilder app)
    {
        // Configure your module-specific middleware here
        app.UseMigration<BasketDbContext>();
        return (app);
    }

}
