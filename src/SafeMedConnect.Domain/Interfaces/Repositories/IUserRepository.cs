using SafeMedConnect.Domain.Entities;

namespace SafeMedConnect.Domain.Interfaces.Repositories;

public interface IUserRepository
{
    public Task<UserEntity?> UpdateUserAsync(UserEntity user, CancellationToken cnl = default);
}