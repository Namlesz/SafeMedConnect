using SafeMedConnect.Domain.Models;

namespace SafeMedConnect.Domain.Interfaces.Services;

public interface ISessionService
{
    public UserClaims GetUserClaims();

    public UserClaims GetGuestClaims();
}