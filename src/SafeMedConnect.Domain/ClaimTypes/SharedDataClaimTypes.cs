namespace SafeMedConnect.Domain.ClaimTypes;

public static class SharedDataClaimTypes
{
    public const string ShareSensitiveData = "share_sensitive_data";
    public const string ShareBloodPressureMeasurement = "share_blood_pressure_measurement";
    public const string ShareHeartRateMeasurement = "share_heart_rate_measurement";
    public const string ShareTemperatureMeasurement = "share_temperature_measurement";
    public const string ShareBloodSugarMeasurement = "share_blood_sugar_measurement";

    public static IEnumerable<string> All =>
    [
        ShareSensitiveData,
        ShareBloodPressureMeasurement,
        ShareHeartRateMeasurement,
        ShareTemperatureMeasurement,
        ShareBloodSugarMeasurement
    ];
}