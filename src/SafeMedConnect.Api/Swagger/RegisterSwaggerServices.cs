using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using SafeMedConnect.Api.Helpers;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace SafeMedConnect.Api.Swagger;

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
        services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
    }
}