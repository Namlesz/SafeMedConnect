using FluentValidation;
using MediatR;
using SafeMedConnect.Domain.Interfaces.Services;
using SafeMedConnect.Domain.Responses;

namespace SafeMedConnect.Application.Commands.Account;

public sealed record VerifyMfaAuthenticatorCommand(string Code) : IRequest<ResponseWrapper>;

public class VerifyMfaAuthenticatorCommandHandler(
    ISessionService session,
    IMfaService mfaService
) : IRequestHandler<VerifyMfaAuthenticatorCommand, ResponseWrapper>
{
    public async Task<ResponseWrapper> Handle(VerifyMfaAuthenticatorCommand request, CancellationToken cancellationToken)
    {
        var userId = session.GetUserClaims().UserId;

        var secret = await mfaService.GetUserMfaSecretAsync(userId, cancellationToken);
        if (secret is null)
        {
            return new ResponseWrapper(ResponseTypes.InvalidRequest, "MFA not configured");
        }

        if (!mfaService.IsCodeValid(request.Code, secret))
        {
            return new ResponseWrapper(ResponseTypes.InvalidRequest, "Invalid MFA code");
        }

        var mfaActivated = await mfaService.ActivateUserMfaAsync(userId, cancellationToken);
        if (!mfaActivated)
        {
            return new ResponseWrapper(ResponseTypes.Error, "Error while activating MFA");
        }

        return new ResponseWrapper(ResponseTypes.Success);
    }
}

public sealed class VerifyMfaAuthenticatorCommandValidator : AbstractValidator<VerifyMfaAuthenticatorCommand>
{
    public VerifyMfaAuthenticatorCommandValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty()
            .Matches(@"^\d{6}$");
    }
}