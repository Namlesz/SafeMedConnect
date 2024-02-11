namespace SafeMedConnect.Application.Dto.Measurements;

public sealed record BloodPressureDto(
    int Systolic,
    int Diastolic,
    int HeartRate,
    DateTime Timestamp
);