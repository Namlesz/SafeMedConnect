using MongoDB.Driver;
using SafeMedConnect.Domain.Abstract.Repositories;
using SafeMedConnect.Domain.Entities;
using SafeMedConnect.Infrastructure.Data;

namespace SafeMedConnect.Infrastructure.Repositories;

internal sealed class MfaRepository(MongoContext db) : IMfaRepository
{
    public async Task<bool> AddMfaSecretToUserAsync(string userId, string secret, CancellationToken cnl = default)
    {
        var updateResult = await db.ApplicationUsers
            .UpdateOneAsync(
                x => x.UserId == userId,
                Builders<ApplicationUserEntity>.Update.Set(x => x.MfaSecret, secret),
                cancellationToken: cnl
            );

        return updateResult.IsAcknowledged && updateResult.ModifiedCount is 1;
    }

    public Task<string?> GetUserMfaSecretAsync(string userId, CancellationToken cnl = default) =>
        db.ApplicationUsers.Find(x => x.UserId == userId)
            .Project(x => x.MfaSecret)
            .FirstOrDefaultAsync(cnl);

    public async Task<bool> ActivateUserMfaAsync(string userId, CancellationToken cnl = default)
    {
        var updateResult = await db.ApplicationUsers
            .UpdateOneAsync(
                x => x.UserId == userId,
                Builders<ApplicationUserEntity>.Update.Set(x => x.MfaEnabled, true),
                cancellationToken: cnl
            );

        return updateResult.IsAcknowledged && updateResult.ModifiedCount is 1;
    }

    public async Task<bool> RemoveMfaFromUserAsync(string userId, CancellationToken cnl = default)
    {
        var updateResult = await db.ApplicationUsers
            .UpdateOneAsync(
                x => x.UserId == userId,
                Builders<ApplicationUserEntity>.Update
                    .Set(x => x.MfaSecret, null)
                    .Set(x => x.MfaEnabled, false),
                cancellationToken: cnl
            );

        return updateResult.IsAcknowledged && updateResult.ModifiedCount is 1;
    }
}