using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using SafeMedConnect.Api.Swagger;
using SafeMedConnect.Api.Swagger.Filters;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace SafeMedConnect.Api.Startup;

internal static class RegisterSwaggerServices
{
    public static void RegisterSwaggerService(this IServiceCollection services)
    {
        services.AddSwaggerGen(config =>
        {
            config.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "SafeMedConnect API",
                Version = "v1",
                Description = "Web api interface for secure management and sharing of medical data."
            });
            config.OperationFilter<BasicAuthOperationsFilter>();
        });
        services.AddTransient<IConfigureOptions<SwaggerGenOptions>, SwaggerGenOptionsConfig>();
    }
}