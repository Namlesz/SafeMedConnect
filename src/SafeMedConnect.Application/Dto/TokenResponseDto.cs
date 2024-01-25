namespace SafeMedConnect.Application.Dto;

public sealed class TokenResponseDto(string token)
{
    public string Token { get; set; } = token;
}