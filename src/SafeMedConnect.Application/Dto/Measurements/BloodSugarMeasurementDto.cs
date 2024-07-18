namespace SafeMedConnect.Application.Dto.Measurements;

public sealed record BloodSugarMeasurementDto(
    string Id,
    decimal Value,
    DateTime Timestamp,
    string Unit,
    string MeasurementMethod
);