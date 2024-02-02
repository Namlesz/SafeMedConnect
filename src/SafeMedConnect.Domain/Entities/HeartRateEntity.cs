using MongoDB.Bson.Serialization.Attributes;
using SafeMedConnect.Domain.Entities.Base;

namespace SafeMedConnect.Domain.Entities;

public sealed class HeartRateEntity : BaseObservationEntity<HeartRateMeasurementEntity>;

public sealed class HeartRateMeasurementEntity : BaseMeasurementEntity
{
    [BsonElement("timestamp")]
    public DateTime Timestamp { get; init; }

    [BsonElement("value")]
    public int Value { get; init; }
}