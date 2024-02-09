using System.ComponentModel.DataAnnotations;

namespace SafeMedConnect.Domain.Configuration;

public sealed class JwtSettings
{
    [Required]
    public string Issuer { get; init; } = null!;

    [Required]
    public string Audience { get; init; } = null!;

    [Required]
    public string Key { get; init; } = null!;

    [Range(1, int.MaxValue)]
    [Required]
    public int ExpirationInMinutes { get; init; }
}