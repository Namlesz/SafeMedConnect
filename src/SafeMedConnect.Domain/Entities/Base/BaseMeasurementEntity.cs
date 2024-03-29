using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SafeMedConnect.Domain.Entities.Base;

public abstract class BaseMeasurementEntity
{
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; init; } = ObjectId.GenerateNewId().ToString();

    [BsonElement("timestamp")]
    public DateTime Timestamp { get; init; }
}