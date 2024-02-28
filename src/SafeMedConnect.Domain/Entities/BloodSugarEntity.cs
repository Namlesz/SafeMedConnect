using SafeMedConnect.Domain.Entities.Base;
using SafeMedConnect.Domain.Enums;

namespace SafeMedConnect.Domain.Entities;

public sealed class BloodSugarEntity : BaseObservationEntity<BloodSugarMeasurementEntity>;

public sealed class BloodSugarMeasurementEntity : BaseMeasurementEntity
{
    public decimal Value { get; init; }
    public GlucoseUnitType Unit { get; init; }
    public BloodSugarMeasurementMethodType MeasurementMethod { get; init; }
}