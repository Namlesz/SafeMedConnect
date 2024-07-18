using System.Reflection;
using SafeMedConnect.Api.Abstract;
using SafeMedConnect.Api.Attributes;

namespace SafeMedConnect.Api.Extensions;

internal static class RoutesExtensions
{
    public static void MapRoutes(this RouteGroupBuilder routeGroupBuilder)
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies().Distinct();
        var routeImplementations = assemblies
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => typeof(IRoutes).IsAssignableFrom(type) && type is { IsInterface: false, IsAbstract: false });

        foreach (var routeImplementation in routeImplementations)
        {
            var routePrefix = routeImplementation.GetCustomAttribute<ApiRouteAttribute>()?.Route ?? string.Empty;

            if (Activator.CreateInstance(routeImplementation) is IRoutes routesInstance)
            {
                routesInstance.RegisterRoutes(
                    routeGroupBuilder.MapGroup(routePrefix).WithOpenApi().WithTags(routePrefix)
                );
            }
        }
    }
}