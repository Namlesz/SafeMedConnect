using FluentValidation;
using MediatR;
using SafeMedConnect.Domain.Abstract.Services;
using SafeMedConnect.Domain.Enums;
using SafeMedConnect.Domain.Models;

namespace SafeMedConnect.Application.Commands.Account;

public sealed record VerifyMfaAuthenticatorCommand(string Code) : IRequest<ApiResponse>;

public class VerifyMfaAuthenticatorCommandHandler(
    ISessionService session,
    IMfaService mfaService
) : IRequestHandler<VerifyMfaAuthenticatorCommand, ApiResponse>
{
    public async Task<ApiResponse> Handle(VerifyMfaAuthenticatorCommand request, CancellationToken cancellationToken)
    {
        var userId = session.GetUserClaims().UserId;

        var secret = await mfaService.GetUserMfaSecretAsync(userId, cancellationToken);
        if (secret is null)
        {
            return new ApiResponse(ApiResponseTypes.InvalidRequest, "MFA not configured");
        }

        if (!mfaService.IsCodeValid(request.Code, secret))
        {
            return new ApiResponse(ApiResponseTypes.InvalidRequest, "Invalid MFA code");
        }

        var mfaActivated = await mfaService.ActivateUserMfaAsync(userId, cancellationToken);
        if (!mfaActivated)
        {
            return new ApiResponse(ApiResponseTypes.Error, "Error while activating MFA");
        }

        return new ApiResponse(ApiResponseTypes.Success);
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