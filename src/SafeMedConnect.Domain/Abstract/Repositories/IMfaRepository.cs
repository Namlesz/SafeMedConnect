namespace SafeMedConnect.Domain.Abstract.Repositories;

public interface IMfaRepository
{
    public Task<bool> AddMfaSecretToUserAsync(string userId, string secret, CancellationToken cnl = default);

    public Task<string?> GetUserMfaSecretAsync(string userId, CancellationToken cnl = default);

    public Task<bool> ActivateUserMfaAsync(string userId, CancellationToken cnl = default);

    public Task<bool> RemoveMfaFromUserAsync(string userId, CancellationToken cnl = default);
}