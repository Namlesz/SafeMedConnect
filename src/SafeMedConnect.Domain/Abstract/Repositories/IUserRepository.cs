using SafeMedConnect.Domain.Entities;

namespace SafeMedConnect.Domain.Abstract.Repositories;

public interface IUserRepository
{
    public Task<UserEntity?> UpdateUserAsync(UserEntity user, CancellationToken cnl = default);

    public Task<UserEntity?> GetUserAsync(string id, CancellationToken cnl = default);
}