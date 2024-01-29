using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SafeMedConnect.Domain.Entities;

public class BaseMeasurementEntity<T> where T : class
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("user_id")]
    public string UserId { get; init; } = null!;

    [BsonElement("measurements")]
    public List<T>? Measurements { get; init; }
}