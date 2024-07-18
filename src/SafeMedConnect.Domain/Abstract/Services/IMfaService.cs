namespace SafeMedConnect.Domain.Abstract.Services;

public interface IMfaService
{
    public bool IsCodeValid(string code, string secret);

    public string GenerateSecretKey();

    public Task<bool> AddMfaSecretToUserAsync(string userId, string secretKey, CancellationToken cancellationToken = default);

    public Task<bool> RemoveMfaFromUserAsync(string userId, CancellationToken cancellationToken = default);

    public Task<string?> GetUserMfaSecretAsync(string userId, CancellationToken cancellationToken = default);

    public Task<bool> ActivateUserMfaAsync(string userId, CancellationToken cancellationToken = default);
}