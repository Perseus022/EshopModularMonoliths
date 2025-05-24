using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Catalog;

public static class CatalogModule
{
    public static IServiceCollection AddCatalogModule(this IServiceCollection services,
        IConfiguration configuration)
    {

        return services;
    }

    public static IApplicationBuilder UseCatalogeModule(this IApplicationBuilder app)
    {
        // Configure your module-specific middleware here
        return (app);
    }
}
