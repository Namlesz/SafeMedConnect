using SafeMedConnect.Api.Attributes;
using SafeMedConnect.Api.Interfaces;
using System.Reflection;

namespace SafeMedConnect.Api.Extensions;

internal static class RoutesInitializer
{
    public static void MapRoutes(this RouteGroupBuilder root)
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();

        var classes = assemblies
            .Distinct()
            .SelectMany(x => x.GetTypes())
            .Where(x =>
                typeof(IRoutes).IsAssignableFrom(x) && x is { IsInterface: false, IsAbstract: false }
            );

        foreach (var classe in classes)
        {
            var routePrefix = classe.GetCustomAttribute<ApiRouteAttribute>()?.Route ?? string.Empty;

            var instance = Activator.CreateInstance(classe) as IRoutes;
            instance?.RegisterRoutes(
                root.MapGroup(routePrefix)
                    .WithOpenApi()
                    .WithTags(routePrefix)
            );
        }
    }
}