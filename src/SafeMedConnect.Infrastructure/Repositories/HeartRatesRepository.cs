using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using SafeMedConnect.Domain.Entities;
using SafeMedConnect.Domain.Interfaces.Repositories;
using SafeMedConnect.Infrastructure.Data;

namespace SafeMedConnect.Infrastructure.Repositories;

//TODO: Remove, use generic class instead
internal sealed class HeartRatesRepository(MongoContext db, ILogger<HeartRatesRepository> logger) : IHeartRatesRepository
{
    private IMongoCollection<HeartRateEntity> Users => db.HeartRates;

    public async Task<HeartRateEntity?> AddHeartRateMeasurementAsync(
        string userId,
        HeartRateMeasurementEntity measurement,
        CancellationToken cnl = default
    )
    {
        var filter = Builders<HeartRateEntity>.Filter.Eq(x => x.UserId, userId);
        var update = Builders<HeartRateEntity>.Update.Push(x => x.Measurements, measurement);
        var options = new FindOneAndUpdateOptions<HeartRateEntity>
        {
            IsUpsert = true,
            ReturnDocument = ReturnDocument.After
        };

        try
        {
            return await Users.FindOneAndUpdateAsync(filter, update, options, cnl);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while adding heart rate measurement");
            return null;
        }
    }

    public async Task<HeartRateEntity?> GetHeartRateMeasurementsAsync(string userId, CancellationToken cnl = default) =>
        await Users.Find(x => x.UserId == userId).FirstOrDefaultAsync(cnl);

    public async Task<HeartRateEntity?> ReplaceHeartRateMeasurementsAsync(
        HeartRateEntity heartRate,
        CancellationToken cnl = default
    )
    {
        var filter = Builders<HeartRateEntity>.Filter.Eq(x => x.UserId, heartRate.UserId);
        var options = new FindOneAndReplaceOptions<HeartRateEntity>
        {
            ReturnDocument = ReturnDocument.After
        };

        try
        {
            return await Users.FindOneAndReplaceAsync(filter, heartRate, options, cancellationToken: cnl);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while replacing heart rate measurements");
            return null;
        }
    }
}