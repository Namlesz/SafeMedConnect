using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using SafeMedConnect.Domain.Entities;
using SafeMedConnect.Domain.Interfaces.Repositories;
using SafeMedConnect.Infrastructure.Data;

namespace SafeMedConnect.Infrastructure.Repositories;

internal sealed class UserRepository(MongoContext db, ILogger<UserRepository> logger) : IUserRepository
{
    private IMongoCollection<UserEntity> Users => db.Users;

    public async Task<UserEntity?> UpdateUserAsync(UserEntity user, CancellationToken cnl = default)
    {
        try
        {
            var result = await Users.ReplaceOneAsync(x => x.Id == user.Id, user, cancellationToken: cnl);
            if (result.IsAcknowledged && result.ModifiedCount == 1)
            {
                return user;
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Exception while updating user");
        }

        logger.LogError("User not updated");
        return null;
    }

    public async Task<UserEntity?> GetUserAsync(string id, CancellationToken cnl = default) =>
        await Users.Find(x => x.Id == id).FirstOrDefaultAsync(cnl);
}