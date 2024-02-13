namespace SafeMedConnect.Domain.Interfaces.Repositories;

public interface IMfaRepository
{
    public Task<bool> AddMfaSecretAsync(string id, string secret, CancellationToken cnl = default);

    public Task<string?> GetMfaSecretAsync(string id, CancellationToken cnl = default);

    public Task<bool> ActivateMfaAsync(string id, CancellationToken cnl = default);
}