using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using SafeMedConnect.Domain.Abstract.Repositories;
using SafeMedConnect.Domain.Entities.Base;
using SafeMedConnect.Infrastructure.Data;
using static SafeMedConnect.Infrastructure.Helpers.RepositoryHelper;

namespace SafeMedConnect.Infrastructure.Repositories;

internal sealed class MeasurementRepository<T>(
    MongoContext db,
    ILogger<MeasurementRepository<T>> logger
) : IMeasurementRepository<T> where T : BaseMeasurementEntity
{
    private readonly IMongoCollection<BaseObservationEntity<T>> _collection =
        db.GetCollection<BaseObservationEntity<T>>(GetCollectionName<T>());

    public async Task<BaseObservationEntity<T>?> GetAsync(string userId, CancellationToken cnl = default) =>
        await _collection.Find(x => x.UserId == userId).FirstOrDefaultAsync(cnl);

    public async Task<BaseObservationEntity<T>?> AddAsync(string userId, T measurement, CancellationToken cnl = default)
    {
        var filter = Builders<BaseObservationEntity<T>>.Filter.Eq(x => x.UserId, userId);
        var update = Builders<BaseObservationEntity<T>>.Update.Push<T>(x => x.Measurements, measurement);
        var options = new FindOneAndUpdateOptions<BaseObservationEntity<T>>
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

    public async Task<BaseObservationEntity<T>?> UpdateAsync(BaseObservationEntity<T> entity, CancellationToken cnl = default)
    {
        var filter = Builders<BaseObservationEntity<T>>.Filter.Eq(x => x.UserId, entity.UserId);
        var options = new FindOneAndReplaceOptions<BaseObservationEntity<T>>
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