using SafeMedConnect.Domain.Models;

namespace SafeMedConnect.Domain.Abstract.Services;

public interface ISessionService
{
    public UserClaims GetUserClaims();

    public GuestClaims GetGuestClaims();
}