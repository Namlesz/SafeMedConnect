using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SafeMedConnect.Domain.Authorization;
using SafeMedConnect.Domain.ClaimTypes;
using SafeMedConnect.Domain.Entities;
using SafeMedConnect.Domain.Interfaces.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SafeMedConnect.Infrastructure.Services;

internal sealed class TokenService : ITokenService
{
    private readonly byte[] _secret;
    private readonly string _issuer;
    private readonly string _audience;
    private readonly DateTime _expiration;

    public TokenService(IConfiguration configuration)
    {
        var jwtSettings = configuration.GetSection("JwtSettings");

        _secret = Encoding.ASCII.GetBytes(jwtSettings["Key"]
            ?? throw new InvalidOperationException("JwtSettings:Key is missing"));

        _expiration = DateTime.UtcNow.AddMinutes(int.Parse(jwtSettings["ExpirationInMinutes"]
            ?? throw new InvalidOperationException("JwtSettings:ExpirationInMinutes is missing")));

        _issuer = jwtSettings["Issuer"]
            ?? throw new InvalidOperationException("JwtSettings:Issuer is missing");

        _audience = jwtSettings["Audience"]
            ?? throw new InvalidOperationException("JwtSettings:Audience is missing");
    }

    public string GenerateJwtToken(ApplicationUserEntity user, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, Roles.User),
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

    public string GenerateDataShareToken(int minutesToExpire, string userId, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Role, Roles.Guest),
                new Claim(UserClaimTypes.UserId, userId)
            }),
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