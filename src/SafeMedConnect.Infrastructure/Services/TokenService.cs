using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SafeMedConnect.Domain.Authorization;
using SafeMedConnect.Domain.Entities;
using SafeMedConnect.Domain.Interfaces.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SafeMedConnect.Infrastructure.Services;

internal sealed class TokenService(IConfiguration configuration) : ITokenService
{
    public string GenerateJwtToken(ApplicationUserEntity user)
    {
        var jwtSettings = configuration.GetSection("JwtSettings");

        var secret = Encoding.ASCII.GetBytes(jwtSettings["Key"]
            ?? throw new InvalidOperationException("JwtSettings:Key is missing"));

        var expiration = DateTime.UtcNow.AddMinutes(int.Parse(jwtSettings["ExpirationInMinutes"]
            ?? throw new InvalidOperationException("JwtSettings:ExpirationInMinutes is missing")));

        var issuer = jwtSettings["Issuer"]
            ?? throw new InvalidOperationException("JwtSettings:Issuer is missing");

        var audience = jwtSettings["Audience"]
            ?? throw new InvalidOperationException("JwtSettings:Audience is missing");

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, Roles.User),
                new Claim(CustomClaimTypes.UserId, user.Id!)
            }),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256Signature
            ),
            Expires = expiration,
            Issuer = issuer,
            Audience = audience
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}