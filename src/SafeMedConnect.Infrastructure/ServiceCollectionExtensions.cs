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

        services.AddScoped<IMeasurementRepository<HeartRateEntity, HeartRateMeasurementEntity>,
            MeasurementRepository<HeartRateEntity, HeartRateMeasurementEntity>>();

        services.AddScoped<IMeasurementRepository<BloodPressureEntity, BloodPressureMeasurementEntity>,
            MeasurementRepository<BloodPressureEntity, BloodPressureMeasurementEntity>>();

        services.AddScoped<IMeasurementRepository<TemperatureEntity, TemperatureMeasurementEntity>,
            MeasurementRepository<TemperatureEntity, TemperatureMeasurementEntity>>();

        services.AddScoped<IMeasurementRepository<BloodSugarEntity, BloodSugarMeasurementEntity>,
            MeasurementRepository<BloodSugarEntity, BloodSugarMeasurementEntity>>();

        // Simplified repositories
        services.AddScoped<IMeasurementRepositorySimplified<HeartRateMeasurementEntity>,
            MeasurementRepositorySimplified<HeartRateMeasurementEntity>>();

        services.AddScoped<IMeasurementRepositorySimplified<BloodPressureMeasurementEntity>,
            MeasurementRepositorySimplified<BloodPressureMeasurementEntity>>();

        services.AddScoped<IMeasurementRepositorySimplified<TemperatureMeasurementEntity>,
            MeasurementRepositorySimplified<TemperatureMeasurementEntity>>();

        services.AddScoped<IMeasurementRepositorySimplified<BloodSugarMeasurementEntity>,
            MeasurementRepositorySimplified<BloodSugarMeasurementEntity>>();
    }
}