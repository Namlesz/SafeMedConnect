using SafeMedConnect.Domain.Entities;

namespace SafeMedConnect.Domain.Interfaces.Repositories;

public interface IUserRepository
{
    public Task<UserEntity?> UpdateUserAsync(UserEntity user, CancellationToken cnl = default);

    public Task<UserEntity?> GetUserAsync(string id, CancellationToken cnl = default);
}