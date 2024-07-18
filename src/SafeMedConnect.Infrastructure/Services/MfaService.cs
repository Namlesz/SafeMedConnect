using OtpNet;
using SafeMedConnect.Domain.Abstract.Repositories;
using SafeMedConnect.Domain.Abstract.Services;

namespace SafeMedConnect.Infrastructure.Services;

internal sealed class MfaService(IMfaRepository repository) : IMfaService
{
    public bool IsCodeValid(string code, string secret)
    {
        var secretKey = Base32Encoding.ToBytes(secret);
        var totp = new Totp(secretKey);
        return totp.VerifyTotp(code, out _);
    }

    public string GenerateSecretKey() =>
        Base32Encoding.ToString(KeyGeneration.GenerateRandomKey(20)).TrimEnd('=');

    public Task<bool> AddMfaSecretToUserAsync(string userId, string secretKey, CancellationToken cancellationToken = default) =>
        repository.AddMfaSecretToUserAsync(userId, secretKey, cancellationToken);

    public Task<bool> RemoveMfaFromUserAsync(string userId, CancellationToken cancellationToken = default) =>
        repository.RemoveMfaFromUserAsync(userId, cancellationToken);

    public Task<string?> GetUserMfaSecretAsync(string userId, CancellationToken cancellationToken = default) =>
        repository.GetUserMfaSecretAsync(userId, cancellationToken);

    public Task<bool> ActivateUserMfaAsync(string userId, CancellationToken cancellationToken = default) =>
        repository.ActivateUserMfaAsync(userId, cancellationToken);
}