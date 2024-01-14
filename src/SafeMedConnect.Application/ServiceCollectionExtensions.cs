using Microsoft.Extensions.DependencyInjection;

namespace SafeMedConnect.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterApplicationServices(this IServiceCollection services)
    {
        return services;
    }
}