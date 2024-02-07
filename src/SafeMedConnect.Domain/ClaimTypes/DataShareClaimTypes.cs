namespace SafeMedConnect.Domain.ClaimTypes;

public static class DataShareClaimTypes
{
    public const string ShareSensitiveData = "share_sensitive_data";
    public const string ShareBloodPressureMeasurement = "share_blood_pressure_measurement";
    public const string ShareHeartRateMeasurement = "share_heart_rate_measurement";

    public static IEnumerable<string> All =>
    [
        ShareSensitiveData,
        ShareBloodPressureMeasurement,
        ShareHeartRateMeasurement
    ];
}