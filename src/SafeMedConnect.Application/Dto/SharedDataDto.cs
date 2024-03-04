using SafeMedConnect.Application.Dto.Measurements;
using System.Text.Json.Serialization;

namespace SafeMedConnect.Application.Dto;

public sealed class SharedDataDto
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public UserDto? UserInformation { get; set; }

    public MeasurementsDto Measurements { get; set; } = new();
}

public sealed class MeasurementsDto
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<HeartRateDto>? HeartRates { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<BloodPressureDto>? BloodPressures { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<TemperatureDto>? Temperatures { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<BloodSugarDto>? BloodSugars { get; set; }
}