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
    }

    private static void RegisterRepositories(this IServiceCollection services)
    {
        services.AddScoped<IApplicationUserRepository, ApplicationUserRepository>();

        services.AddScoped<IUserRepository, UserRepository>();

        services.AddScoped<IMfaRepository, MfaRepository>();

        services.AddScoped<IMeasurementRepository<HeartRateEntity, HeartRateMeasurementEntity>,
            MeasurementRepository<HeartRateEntity, HeartRateMeasurementEntity>>();

        services.AddScoped<IMeasurementRepository<BloodPressureEntity, BloodPressureMeasurementEntity>,
            MeasurementRepository<BloodPressureEntity, BloodPressureMeasurementEntity>>();

        services.AddScoped<IMeasurementRepository<TemperatureEntity, TemperatureMeasurementEntity>,
            MeasurementRepository<TemperatureEntity, TemperatureMeasurementEntity>>();
    }
}