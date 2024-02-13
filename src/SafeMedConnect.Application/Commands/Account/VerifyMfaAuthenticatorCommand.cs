using MediatR;
using OtpNet;
using SafeMedConnect.Domain.Interfaces.Repositories;
using SafeMedConnect.Domain.Interfaces.Services;
using SafeMedConnect.Domain.Responses;

namespace SafeMedConnect.Application.Commands.Account;

public sealed record VerifyMfaAuthenticatorCommand(string Code) : IRequest<ResponseWrapper>;

public class VerifyMfaAuthenticatorCommandHandler(
    ISessionService session,
    IMfaRepository repository
) : IRequestHandler<VerifyMfaAuthenticatorCommand, ResponseWrapper>
{
    public async Task<ResponseWrapper> Handle(VerifyMfaAuthenticatorCommand request, CancellationToken cancellationToken)
    {
        var userId = session.GetUserClaims().UserId;

        var secret = await repository.GetMfaSecretAsync(userId, cancellationToken);
        if (secret is null)
        {
            return new ResponseWrapper(ResponseTypes.InvalidRequest, "MFA not configured");
        }

        if (!IsCodeValid(request.Code, secret))
        {
            return new ResponseWrapper(ResponseTypes.InvalidRequest, "Invalid MFA code");
        }

        var mfaActivated = await repository.ActivateMfaAsync(userId, cancellationToken);
        if (!mfaActivated)
        {
            return new ResponseWrapper(ResponseTypes.Error, "Error while activating MFA");
        }

        return new ResponseWrapper(ResponseTypes.Success);
    }

    private static bool IsCodeValid(string code, string secret)
    {
        var secretKey = Base32Encoding.ToBytes(secret);
        var totp = new Totp(secretKey);
        // return totp.VerifyTotp(code, out _, new VerificationWindow(2, 2));
        return totp.VerifyTotp(code, out _);
    }
}