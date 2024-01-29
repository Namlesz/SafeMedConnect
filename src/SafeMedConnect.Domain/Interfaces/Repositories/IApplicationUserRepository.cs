using SafeMedConnect.Domain.Entities;

namespace SafeMedConnect.Domain.Interfaces.Repositories;

public interface IApplicationUserRepository
{
    public Task<bool> RegisterUserAsync(ApplicationUserEntity appUser, CancellationToken cnl = default);

    public Task<ApplicationUserEntity?> GetUserAsync(string email, CancellationToken cnl = default);
}