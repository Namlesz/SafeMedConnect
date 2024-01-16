using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using SafeMedConnect.Application.Commands;
using SafeMedConnect.Application.Validators;

namespace SafeMedConnect.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterApplicationServices(this IServiceCollection services)
    {
        services.RegisterMediatR();
        services.RegisterFluentValidation();
        return services;
    }

    private static void RegisterFluentValidation(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<RegisterApplicationUserCommandValidator>(ServiceLifetime.Singleton);
    }

    private static void RegisterMediatR(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(RegisterApplicationUserCommand).Assembly);
        });
    }
}