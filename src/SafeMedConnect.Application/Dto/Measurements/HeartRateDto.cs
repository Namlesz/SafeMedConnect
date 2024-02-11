namespace SafeMedConnect.Application.Dto.Measurements;

public sealed record HeartRateDto(
    int Value,
    DateTime Timestamp
);