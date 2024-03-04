using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SafeMedConnect.Domain.Configuration;
using SafeMedConnect.Domain.Entities;
using SafeMedConnect.Domain.Interfaces.Repositories;
using SafeMedConnect.Domain.Interfaces.Services;
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

        services.AddScoped<IMeasurementRepository<HeartRateMeasurementEntity>,
            MeasurementRepository<HeartRateMeasurementEntity>>();

        services.AddScoped<IMeasurementRepository<BloodPressureMeasurementEntity>,
            MeasurementRepository<BloodPressureMeasurementEntity>>();

        services.AddScoped<IMeasurementRepository<TemperatureMeasurementEntity>,
            MeasurementRepository<TemperatureMeasurementEntity>>();

        services.AddScoped<IMeasurementRepository<BloodSugarMeasurementEntity>,
            MeasurementRepository<BloodSugarMeasurementEntity>>();
    }
}