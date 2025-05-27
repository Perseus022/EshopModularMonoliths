using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Data.Interceptors;


namespace Catalog;

public static class CatalogModule
{
    public static IServiceCollection AddCatalogModule(this IServiceCollection services,
        IConfiguration configuration)
    {
        // Add Services to the Container

        //Api Entpoint Services

        //Application use cases Services
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });

        //Data Infrastructure Services
        var connectionString = configuration.GetConnectionString("Database");

        services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
        services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterseptor>();

        services.AddDbContext<CatalogDbContext>((sp, options) =>
        {
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
            options.UseNpgsql(connectionString);
        });
        services.AddScoped<IDataSeeder, CatalogDataSeeder>();
        return services;

    }

    public static IApplicationBuilder UseCatalogeModule(this IApplicationBuilder app)
    {
        // Configure Http Request Pipeline

        // 1. Use Api Endpoint Services

        // 2. Use Application Use Cases Services

        // 3. Use Data - Infrastructure Services
        app.UseMigration<CatalogDbContext>();
        return (app);
    }

}
