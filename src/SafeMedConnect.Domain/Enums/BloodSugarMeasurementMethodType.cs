namespace SafeMedConnect.Domain.Enums;

public enum BloodSugarMeasurementMethodType
{
    Random,
    Fasting,
    Postprandial
}

public static class BloodSugarMeasurementMethodTypeExtensions
{
    private static readonly Dictionary<BloodSugarMeasurementMethodType, string> DisplayNames = new()
    {
        { BloodSugarMeasurementMethodType.Fasting, "na czczo" },
        { BloodSugarMeasurementMethodType.Postprandial, "po posiłku" },
        { BloodSugarMeasurementMethodType.Random, "losowy pomiar" }
    };

    public static string? GetDisplayName(this BloodSugarMeasurementMethodType type) =>
        DisplayNames.GetValueOrDefault(type);
}