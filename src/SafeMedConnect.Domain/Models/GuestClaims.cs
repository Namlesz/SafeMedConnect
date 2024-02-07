namespace SafeMedConnect.Domain.Models;

public sealed class GuestClaims
{
    public string UserId { get; init; } = null!;
    public string Role { get; init; } = null!;
    public Dictionary<string, bool> DataShareClaims { get; init; } = null!;
}