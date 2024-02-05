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
        var httpContext = httpContextAccessor.HttpContext
            ?? throw new NullReferenceException("HttpContext is null");

        var claims = httpContext.User.Claims.ToList();

        var userIdClaim = claims.FirstOrDefault(c => c.Type == CustomClaimTypes.UserId)
            ?? throw new NullReferenceException("UserId claim is null");

        var emailClaim = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)
            ?? throw new NullReferenceException("Email claim is null");

        var roleClaim = claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)
            ?? throw new NullReferenceException("Role claim is null");

        return new UserClaims
        {
            UserId = userIdClaim.Value,
            Email = emailClaim.Value,
            Role = roleClaim.Value
        };
    }

    public UserClaims GetGuestClaims()
    {
        var httpContext = httpContextAccessor.HttpContext
            ?? throw new NullReferenceException("HttpContext is null");

        var claims = httpContext.User.Claims.ToList();

        var userIdClaim = claims.FirstOrDefault(c => c.Type == CustomClaimTypes.UserId)
            ?? throw new NullReferenceException("UserId claim is null");

        var roleClaim = claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)
            ?? throw new NullReferenceException("Role claim is null");

        return new UserClaims
        {
            UserId = userIdClaim.Value,
            Role = roleClaim.Value
        };
    }
}