namespace SafeMedConnect.Application.Dto;

public sealed record UserDto(
    string? FirstName,
    string? LastName,
    DateTime? DateOfBirth,
    double? Weight,
    double? Height,
    string? BloodType,
    string? HealthInsuranceNumber,
    IEnumerable<string>? Allergies,
    IEnumerable<string>? Medications,
    IEnumerable<string>? DiagnosedDiseases
);