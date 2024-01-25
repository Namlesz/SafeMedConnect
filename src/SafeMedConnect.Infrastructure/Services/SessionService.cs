using Microsoft.AspNetCore.Http;
using SafeMedConnect.Domain.Authorization;
using SafeMedConnect.Domain.Interfaces.Services;
using SafeMedConnect.Domain.Models;
using System.Security.Claims;

namespace SafeMedConnect.Infrastructure.Services;

internal sealed class SessionService(IHttpContextAccessor httpContextAccessor) : ISessionService
{
    public UserClaims GetUserClaims()
    {
        var httpContext = httpContextAccessor.HttpContext ?? throw new NullReferenceException("HttpContext is null");
        var claims = httpContext.User.Claims.ToList();

        return new UserClaims
        {
            UserId = claims.FirstOrDefault(c => c.Type == CustomClaimTypes.UserId)?.Value,
            Email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value,
            Role = claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value
        };
    }
}