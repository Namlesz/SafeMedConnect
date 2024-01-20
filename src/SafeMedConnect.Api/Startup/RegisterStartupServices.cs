using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using SafeMedConnect.Api.Helpers;
using SafeMedConnect.Api.Interfaces;
using SafeMedConnect.Api.Swagger;
using SafeMedConnect.Domain.Authorization;
using System.Text;

namespace SafeMedConnect.Api.Startup;

internal static class RegisterStartupServices
{
    public static void RegisterApiServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddAuthenticationWithJwt(configuration);
        services.AddAuthorizationWithPolicy();

        services.AddEndpointsApiExplorer();
        services.RegisterSwaggerService();

        services.AddProblemDetails();
        services.AddScoped<IResponseHandler, ResponseHandler>();
    }

    private static void AddAuthorizationWithPolicy(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy(PolicyNames.UserPolicy, policy =>
            {
                policy.RequireClaim(ClaimNames.Role, Roles.User);
            });

            options.AddPolicy(PolicyNames.GuestPolicy, policy =>
            {
                policy.RequireClaim(ClaimNames.Role, Roles.Guest);
            });

            options.DefaultPolicy = options.GetPolicy(PolicyNames.UserPolicy)
                ?? throw new InvalidOperationException("UserPolicy is null");
        });
    }

    private static void AddAuthenticationWithJwt(this IServiceCollection services, IConfiguration configuration)
    {
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
                ValidIssuer = configuration["JwtSettings:Issuer"],
                ValidAudience = configuration["JwtSettings:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(configuration["JwtSettings:Key"]
                        ?? throw new InvalidOperationException("JwtSettings:Key is null"))
                ),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
            };
        });
    }
}