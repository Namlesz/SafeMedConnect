using MongoDB.Bson.Serialization.Attributes;
using SafeMedConnect.Domain.Entities.Base;

namespace SafeMedConnect.Domain.Entities;

public sealed class BloodPressureEntity : BaseObservationEntity<BloodPressureMeasurementEntity>;

public sealed class BloodPressureMeasurementEntity : BaseMeasurementEntity
{
    [BsonElement("systolic")]
    public int Systolic { get; init; }

    [BsonElement("diastolic")]
    public int Diastolic { get; init; }

    [BsonElement("heart_rate")]
    public int HeartRate { get; init; }
}