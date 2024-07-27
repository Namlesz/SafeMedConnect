using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SafeMedConnect.Domain.Abstract.Repositories;
using SafeMedConnect.Domain.Abstract.Services;
using SafeMedConnect.Infrastructure.Configuration;
using SafeMedConnect.Infrastructure.Data;
using SafeMedConnect.Infrastructure.Repositories;
using SafeMedConnect.Infrastructure.Services;

namespace SafeMedConnect.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static void RegisterInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptionsWithValidateOnStart<MongoSettings>()
            .Bind(configuration.GetSection("MongoCluster"))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddSingleton<MongoContext>();
        services.RegisterRepositories();
        services.AddCustomServices();
    }

    private static void AddCustomServices(this IServiceCollection services)
    {
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<ISessionService, SessionService>();
        services.AddScoped<IMfaService, MfaService>();
    }

    private static void RegisterRepositories(this IServiceCollection services)
    {
        services.AddScoped<IApplicationUserRepository, ApplicationUserRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IMfaRepository, MfaRepository>();
        services.AddScoped(typeof(IMeasurementRepository<>), typeof(MeasurementRepository<>));
    }
}