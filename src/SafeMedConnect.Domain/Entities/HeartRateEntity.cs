using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SafeMedConnect.Domain.Entities;

public sealed class HeartRateEntity
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("user_id")]
    public string UserId { get; init; } = null!;

    [BsonElement("measurements")]
    public List<HeartRateMeasurementEntity>? Measurements { get; init; }
}

public sealed class HeartRateMeasurementEntity
{
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

    [BsonElement("timestamp")]
    public DateTime Timestamp { get; init; }

    [BsonElement("value")]
    public int Value { get; init; }
}