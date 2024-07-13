using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SafeMedConnect.Api.Helpers;
using SafeMedConnect.Api.Interfaces;
using SafeMedConnect.Api.Swagger;
using SafeMedConnect.Domain.Authorization;
using SafeMedConnect.Domain.Configuration;

namespace SafeMedConnect.Api.Startup;

internal static class RegisterStartupServices
{
    public static void RegisterApiServices(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddAuthenticationWithJwt(configuration);
        services.AddAuthorizationWithPolicy();

        services.AddEndpointsApiExplorer();
        services.RegisterSwaggerService();

        services.AddProblemDetails();
        services.AddScoped<IResponseHandler, ResponseHandler>();
        services.AddHttpContextAccessor();

        services.AddCors();
    }

    private static void AddAuthorizationWithPolicy(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy(PolicyNames.UserPolicy, policy =>
            {
                policy.RequireRole(Roles.User);
            });

            options.AddPolicy(PolicyNames.GuestPolicy, policy =>
            {
                policy.RequireRole(Roles.Guest);
            });

            options.DefaultPolicy = options.GetPolicy(PolicyNames.UserPolicy)
                ?? throw new InvalidOperationException("UserPolicy is null");
        });
    }

    private static void AddAuthenticationWithJwt(this IServiceCollection services, IConfiguration configuration)
    {
        // Add IOptions pattern and validate, same fields are used below in bearer settings
        services.AddOptionsWithValidateOnStart<JwtSettings>()
            .Bind(configuration.GetSection("JwtSettings"))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }
        ).AddJwtBearer(x =>
        {
            x.TokenValidationParameters = new TokenValidationParameters
            {
                // Fields are validated by IOptions pattern
                ValidIssuer = configuration["JwtSettings:Issuer"]!,
                ValidAudience = configuration["JwtSettings:Audience"]!,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:Key"]!)),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ClockSkew = TimeSpan.Zero
            };
            x.Events = new JwtBearerEvents
            {
                OnChallenge = async context =>
                {
                    context.HandleResponse();

                    context.Response.ContentType = "application/json";
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsJsonAsync(new ProblemDetails
                    {
                        Status = StatusCodes.Status401Unauthorized,
                        Title = "Unauthorized",
                        Detail = "Missing or invalid token",
                        Instance = context.Request.Path
                    });
                }
            };
        });
    }
}