using SafeMedConnect.Domain.Entities;

namespace SafeMedConnect.Domain.Interfaces.Repositories;

public interface IApplicationUserRepository
{
    public Task<bool> RegisterUserAsync(ApplicationUserEntity user, CancellationToken cnl = default);

    public Task<ApplicationUserEntity?> GetUserAsync(string login, CancellationToken cnl = default);
}