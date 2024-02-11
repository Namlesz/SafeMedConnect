namespace SafeMedConnect.Application.Dto;

public sealed record BloodPressureDto(
    int Systolic,
    int Diastolic,
    int HeartRate,
    DateTime Timestamp
);