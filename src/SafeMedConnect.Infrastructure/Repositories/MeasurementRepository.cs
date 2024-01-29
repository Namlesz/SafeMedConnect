using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using SafeMedConnect.Domain.Entities;
using SafeMedConnect.Domain.Interfaces.Repositories;
using SafeMedConnect.Infrastructure.Data;
using static SafeMedConnect.Common.Utilities.RepositoryHelper;

namespace SafeMedConnect.Infrastructure.Repositories;

internal sealed class MeasurementRepository<TA, TB>(MongoContext db, ILogger<MeasurementRepository<TA, TB>> logger)
    : IMeasurementRepository<TA, TB> where TA : BaseMeasurementEntity<TB> where TB : class
{
    private readonly IMongoCollection<TA> _collection = db.GetCollection<TA>(GetCollectionName<TA>());

    public async Task<TA?> GetAsync(string userId, CancellationToken cnl = default) =>
        await _collection.Find(x => x.UserId == userId).FirstOrDefaultAsync(cnl);


    public async Task<TA?> AddOrUpdateAsync(string userId, TB measurement, CancellationToken cnl = default)
    {
        var filter = Builders<TA>.Filter.Eq(x => x.UserId, userId);
        var update = Builders<TA>.Update.Push<TB>(x => x.Measurements, measurement);
        var options = new FindOneAndUpdateOptions<TA>
        {
            IsUpsert = true,
            ReturnDocument = ReturnDocument.After
        };

        try
        {
            return await _collection.FindOneAndUpdateAsync(filter, update, options, cnl);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while adding measurement");
            return null;
        }
    }

    public async Task<TA?> ReplaceAsync(TA entity, CancellationToken cnl = default)
    {
        var filter = Builders<TA>.Filter.Eq(x => x.UserId, entity.UserId);
        var options = new FindOneAndReplaceOptions<TA>
        {
            ReturnDocument = ReturnDocument.After
        };

        try
        {
            return await _collection.FindOneAndReplaceAsync(filter, entity, options, cnl);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while replacing measurement");
            return null;
        }
    }
}