namespace SafeMedConnect.Application.Dto.Measurements;

public sealed record BloodSugarDto(
    decimal Value,
    DateTime Timestamp,
    string Unit,
    string MeasurementMethod
);