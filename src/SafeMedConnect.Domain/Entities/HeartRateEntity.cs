using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SafeMedConnect.Domain.Entities;

public sealed class HeartRateEntity : BaseMeasurementEntity<HeartRateMeasurementEntity>;

public sealed class HeartRateMeasurementEntity
{
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; init; } = ObjectId.GenerateNewId().ToString();

    [BsonElement("timestamp")]
    public DateTime Timestamp { get; init; }

    [BsonElement("value")]
    public int Value { get; init; }
}