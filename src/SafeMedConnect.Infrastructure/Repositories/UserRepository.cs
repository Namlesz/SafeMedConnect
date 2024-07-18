using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using SafeMedConnect.Domain.Abstract.Repositories;
using SafeMedConnect.Domain.Entities;
using SafeMedConnect.Infrastructure.Data;

namespace SafeMedConnect.Infrastructure.Repositories;

internal sealed class UserRepository(MongoContext db, ILogger<UserRepository> logger) : IUserRepository
{
    private IMongoCollection<UserEntity> Users => db.Users;

    public async Task<UserEntity?> UpdateUserAsync(UserEntity user, CancellationToken cnl = default)
    {
        var filter = Builders<UserEntity>.Filter.Eq(x => x.Id, user.Id);
        var options = new FindOneAndReplaceOptions<UserEntity>
        {
            ReturnDocument = ReturnDocument.After
        };

        try
        {
            return await Users.FindOneAndReplaceAsync(filter, user, options, cnl);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Exception while updating user");
            return null;
        }
    }

    public async Task<UserEntity?> GetUserAsync(string id, CancellationToken cnl = default) =>
        await Users.Find(x => x.Id == id).FirstOrDefaultAsync(cnl);
}