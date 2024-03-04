namespace SafeMedConnect.Domain.Enums;

public enum GlucoseUnitType
{
    Millimole,
    Milligram
}

public static class GlucoseUnitTypeExtensions
{
    private static readonly Dictionary<GlucoseUnitType, string> DisplayNames = new()
    {
        { GlucoseUnitType.Millimole, "mmol/L" },
        { GlucoseUnitType.Milligram, "mg/dL" }
    };

    public static string? GetDisplayName(this GlucoseUnitType type) =>
        DisplayNames.GetValueOrDefault(type);
}