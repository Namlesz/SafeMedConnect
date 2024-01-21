using Microsoft.Extensions.DependencyInjection;
using SafeMedConnect.Domain.Interfaces.Repositories;
using SafeMedConnect.Domain.Interfaces.Services;
using SafeMedConnect.Infrastructure.Data;
using SafeMedConnect.Infrastructure.Repositories;
using SafeMedConnect.Infrastructure.Services;

namespace SafeMedConnect.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static void RegisterInfrastructureServices(this IServiceCollection services)
    {
        services.AddSingleton<MongoContext>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IApplicationUserRepository, ApplicationUserRepository>();
        services.AddScoped<ITokenService, TokenService>();
    }
}