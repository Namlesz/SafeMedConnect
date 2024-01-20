using Microsoft.OpenApi.Models;
using SafeMedConnect.Api.Helpers;
using SafeMedConnect.Api.Interfaces;

namespace SafeMedConnect.Api.Startup;

internal static class RegisterStartupServices
{
    public static IServiceCollection RegisterApiServices(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(config =>
        {
            config.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "SafeMedConnect API",
                Version = "v1",
                Description = "Web api interface for secure management and sharing of medical data."
            });
        });

        services.AddProblemDetails();
        services.AddScoped<IResponseHandler, ResponseHandler>();
        return services;
    }
}