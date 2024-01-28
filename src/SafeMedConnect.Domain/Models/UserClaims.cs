namespace SafeMedConnect.Domain.Models;

public sealed class UserClaims
{
    public string UserId { get; init; } = null!;
    public string Email { get; init; } = null!;
    public string Role { get; init; } = null!;
}