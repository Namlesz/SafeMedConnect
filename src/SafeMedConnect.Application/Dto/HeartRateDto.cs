namespace SafeMedConnect.Application.Dto;

public sealed record HeartRateDto(
    int Value,
    DateTime Timestamp
);