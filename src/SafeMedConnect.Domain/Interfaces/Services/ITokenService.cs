using SafeMedConnect.Domain.Entities;

namespace SafeMedConnect.Domain.Interfaces.Services;

public interface ITokenService
{
    public string GenerateJwtToken(ApplicationUserEntity user, CancellationToken cancellationToken = default);

    public string GenerateDataShareToken(int minutesToExpire, string userId, CancellationToken cancellationToken = default);
}