using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SafeMedConnect.Domain.Abstract.Services;
using SafeMedConnect.Domain.Auth;
using SafeMedConnect.Domain.ClaimTypes;
using SafeMedConnect.Domain.Entities;
using SafeMedConnect.Infrastructure.Configuration;

namespace SafeMedConnect.Infrastructure.Services;

internal sealed class TokenService : ITokenService
{
    private readonly string _audience;
    private readonly DateTime _expiration;
    private readonly string _issuer;
    private readonly byte[] _secret;

    public TokenService(IOptions<JwtSettings> jwtSettingsOptions)
    {
        var jwtSettings = jwtSettingsOptions.Value;

        _secret = Encoding.ASCII.GetBytes(jwtSettings.Key);
        _expiration = DateTime.UtcNow.AddMinutes(jwtSettings.ExpirationInMinutes);
        _issuer = jwtSettings.Issuer;
        _audience = jwtSettings.Audience;
    }

    public string GenerateJwtToken(ApplicationUserEntity user, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, AppRoles.User),
                new Claim(UserClaimTypes.UserId, user.UserId)
            }),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(_secret), SecurityAlgorithms.HmacSha256Signature
            ),
            Expires = _expiration,
            Issuer = _issuer,
            Audience = _audience
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public string GenerateShareToken(
        int minutesToExpire,
        string userId,
        IEnumerable<Claim>? claims = default,
        CancellationToken cancellationToken = default
    )
    {
        cancellationToken.ThrowIfCancellationRequested();
        var allClaims = new List<Claim>
        {
            new(ClaimTypes.Role, AppRoles.Guest),
            new(UserClaimTypes.UserId, userId)
        }.Concat(claims ?? []);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(allClaims),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(_secret), SecurityAlgorithms.HmacSha256Signature
            ),
            Expires = DateTime.UtcNow.AddMinutes(minutesToExpire),
            Issuer = _issuer,
            Audience = _audience
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}