using SafeMedConnect.Domain.Entities;

namespace SafeMedConnect.Domain.Interfaces.Services;

public interface ITokenService
{
    public string GenerateJwtToken(ApplicationUserEntity user);
}