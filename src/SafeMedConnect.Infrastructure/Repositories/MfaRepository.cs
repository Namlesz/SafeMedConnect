using MongoDB.Driver;
using SafeMedConnect.Domain.Entities;
using SafeMedConnect.Domain.Interfaces.Repositories;
using SafeMedConnect.Infrastructure.Data;

namespace SafeMedConnect.Infrastructure.Repositories;

internal sealed class MfaRepository(MongoContext db) : IMfaRepository
{
    public async Task<bool> AddMfaSecretAsync(string id, string secret, CancellationToken cnl = default)
    {
        var updateResult = await db.ApplicationUsers
            .UpdateOneAsync(
                x => x.Id == id,
                Builders<ApplicationUserEntity>.Update.Set(x => x.MfaSecret, secret),
                cancellationToken: cnl
            );

        return updateResult.IsAcknowledged && updateResult.ModifiedCount is 1;
    }

    public Task<string?> GetMfaSecretAsync(string id, CancellationToken cnl = default) =>
        db.ApplicationUsers.Find(x => x.Id == id)
            .Project(x => x.MfaSecret)
            .FirstOrDefaultAsync(cnl);

    public async Task<bool> ActivateMfaAsync(string id, CancellationToken cnl = default)
    {
        var updateResult = await db.ApplicationUsers
            .UpdateOneAsync(
                x => x.Id == id,
                Builders<ApplicationUserEntity>.Update.Set(x => x.MfaEnabled, true),
                cancellationToken: cnl
            );

        return updateResult.IsAcknowledged && updateResult.ModifiedCount is 1;
    }
}