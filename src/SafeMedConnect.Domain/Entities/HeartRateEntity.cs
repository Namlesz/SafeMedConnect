using MongoDB.Bson.Serialization.Attributes;
using SafeMedConnect.Domain.Attributes;
using SafeMedConnect.Domain.Entities.Base;

namespace SafeMedConnect.Domain.Entities;

public sealed class HeartRateEntity : BaseObservationEntity<HeartRateMeasurementEntity>;

[RepositoryName("HeartRates")]
public sealed class HeartRateMeasurementEntity : BaseMeasurementEntity
{
    [BsonElement("value")]
    public int Value { get; init; }
}