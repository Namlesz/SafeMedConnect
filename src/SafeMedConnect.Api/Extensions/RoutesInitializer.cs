using SafeMedConnect.Api.Interfaces;

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
            var instance = Activator.CreateInstance(classe) as IRoutes;
            instance?.RegisterRoutes(root);
        }
    }
}