namespace SafeMedConnect.Domain.Models;

public sealed class UserClaims
{
    public string? UserId { get; set; }
    public string? Email { get; set; }
    public string? Role { get; set; }
}