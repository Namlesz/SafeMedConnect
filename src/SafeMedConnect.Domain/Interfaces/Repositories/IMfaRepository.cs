namespace SafeMedConnect.Domain.Interfaces.Repositories;

public interface IMfaRepository
{
    public Task<bool> AddMfaSecretAsync(string userId, string secret, CancellationToken cnl = default);

    public Task<string?> GetMfaSecretAsync(string userId, CancellationToken cnl = default);

    public Task<bool> ActivateMfaAsync(string userId, CancellationToken cnl = default);

    public Task<bool> DeactivateMfaAsync(string userId, CancellationToken cnl = default);
}