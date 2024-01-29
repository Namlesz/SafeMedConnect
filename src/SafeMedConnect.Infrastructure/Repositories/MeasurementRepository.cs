using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using SafeMedConnect.Domain.Entities;
using SafeMedConnect.Domain.Interfaces.Repositories;
using SafeMedConnect.Infrastructure.Data;

namespace SafeMedConnect.Infrastructure.Repositories;

internal sealed class MeasurementRepository<TA, TB>
    : IMeasurementRepository<TA, TB> where TA : BaseMeasurementEntity<TB> where TB : class
{
    private readonly IMongoCollection<TA> _collection;
    private readonly ILogger<MeasurementRepository<TA, TB>> _logger;

    public MeasurementRepository(MongoContext db, ILogger<MeasurementRepository<TA, TB>> logger)
    {
        var repositoryName = typeof(TA).Name.Replace("Entity", "s");
        _collection = db.GetCollection<TA>(repositoryName);
        _logger = logger;
    }

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
            _logger.LogError(ex, "Error while adding measurement");
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
            return await _collection.FindOneAndReplaceAsync(filter, entity, options, cancellationToken: cnl);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while replacing heart rate measurements");
            return null;
        }
    }
}