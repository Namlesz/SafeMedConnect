namespace SafeMedConnect.Application.Dto.Measurements;

public sealed record TemperatureDto(
    int Value,
    DateTime Timestamp
);