using SafeMedConnect.Domain.Entities;
using System.Security.Claims;

namespace SafeMedConnect.Domain.Interfaces.Services;

public interface ITokenService
{
    public string GenerateJwtToken(ApplicationUserEntity user, CancellationToken cancellationToken = default);

    public string GenerateShareToken(
        int minutesToExpire,
        string userId,
        IEnumerable<Claim>? claims = default,
        CancellationToken cancellationToken = default
    );
}