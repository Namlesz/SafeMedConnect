using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SafeMedConnect.Domain.Entities;

public sealed class BloodPressureEntity : BaseMeasurementEntity<BloodPressureMeasurementEntity>;

public sealed class BloodPressureMeasurementEntity
{
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

    [BsonElement("systolic")]
    public int Systolic { get; init; }

    [BsonElement("diastolic")]
    public int Diastolic { get; init; }

    [BsonElement("heart_rate")]
    public int HeartRate { get; init; }

    [BsonElement("timestamp")]
    public DateTime Timestamp { get; init; }
}