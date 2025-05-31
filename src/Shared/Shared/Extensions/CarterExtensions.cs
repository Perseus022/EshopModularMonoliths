using Carter;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Shared.Extensions;

public static class CarterExtensions
{
    public static IServiceCollection AddCarterWithAssemlies
        (this IServiceCollection services, params Assembly[] assemblies)
    {
        services.AddCarter(configurator: config =>
        {
            foreach (var assembly in assemblies)
            {
               var modules = assembly.GetTypes()
                    .Where(type => type.IsAssignableTo(typeof(ICarterModule))).ToArray();

                config.WithModules(modules);
            }
        });
        return services;
    }
}