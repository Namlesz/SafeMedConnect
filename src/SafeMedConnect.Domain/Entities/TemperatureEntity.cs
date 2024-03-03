using SafeMedConnect.Domain.Attributes;
using SafeMedConnect.Domain.Entities.Base;

namespace SafeMedConnect.Domain.Entities;

public sealed class TemperatureEntity : BaseObservationEntity<TemperatureMeasurementEntity>;

[RepositoryName("Temperatures")]
public sealed class TemperatureMeasurementEntity : BaseMeasurementEntity
{
    public double Value { get; init; }
}