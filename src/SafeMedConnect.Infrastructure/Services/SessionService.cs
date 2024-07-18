using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using SafeMedConnect.Domain.Abstract.Services;
using SafeMedConnect.Domain.ClaimTypes;
using SafeMedConnect.Domain.Models;

namespace SafeMedConnect.Infrastructure.Services;

internal sealed class SessionService(IHttpContextAccessor httpContextAccessor) : ISessionService
{
    public UserClaims GetUserClaims()
    {
        var httpContext = httpContextAccessor.HttpContext
            ?? throw new NullReferenceException("HttpContext is null");

        var claims = httpContext.User.Claims.ToList();

        var userIdClaim = claims.FirstOrDefault(c => c.Type == UserClaimTypes.UserId)
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

    public GuestClaims GetGuestClaims()
    {
        var httpContext = httpContextAccessor.HttpContext
            ?? throw new NullReferenceException("HttpContext is null");

        var claims = httpContext.User.Claims.ToList();

        var userIdClaim = claims.FirstOrDefault(c => c.Type == UserClaimTypes.UserId)
            ?? throw new NullReferenceException("UserId claim is null");

        var roleClaim = claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)
            ?? throw new NullReferenceException("Role claim is null");

        var dataShareClaims = new Dictionary<string, bool>();

        foreach (var shareClaimType in SharedDataClaimTypes.All)
        {
            var shareClaim = claims.FirstOrDefault(c => c.Type == shareClaimType);
            if (shareClaim is null)
            {
                continue;
            }

            bool.TryParse(shareClaim.Value, out var shareClaimValue);
            dataShareClaims.Add(shareClaimType, shareClaimValue);
        }

        return new GuestClaims
        {
            UserId = userIdClaim.Value,
            Role = roleClaim.Value,
            DataShareClaims = dataShareClaims
        };
    }
}