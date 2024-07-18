using System.Security.Claims;
using SafeMedConnect.Domain.Entities;

namespace SafeMedConnect.Domain.Abstract.Services;

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