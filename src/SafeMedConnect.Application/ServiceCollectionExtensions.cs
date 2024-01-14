using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using SafeMedConnect.Application.Queries;
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

    // TODO: Fix, now it's not auto validate data
    private static void RegisterFluentValidation(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<GetAccountNameQueryValidator>();
    }

    private static void RegisterMediatR(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(GetAccountNameQuery).Assembly);
        });
    }
}