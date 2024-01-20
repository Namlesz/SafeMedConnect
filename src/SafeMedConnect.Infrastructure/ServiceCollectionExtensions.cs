using Microsoft.Extensions.DependencyInjection;
using SafeMedConnect.Domain.Interfaces.Repositories;
using SafeMedConnect.Infrastructure.Data;
using SafeMedConnect.Infrastructure.Repositories;

namespace SafeMedConnect.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static void RegisterInfrastructureServices(this IServiceCollection services)
    {
        services.AddSingleton<MongoContext>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IApplicationUserRepository, ApplicationUserRepository>();
    }
}