using MediatR;
using SafeMedConnect.Domain.Abstract.Services;
using SafeMedConnect.Domain.Enums;
using SafeMedConnect.Domain.Models;

namespace SafeMedConnect.Application.Commands.Account;

public sealed record RemoveMfaAuthenticatorCommand : IRequest<ApiResponse>;

public class RemoveMfaAuthenticatorCommandHandler(
    ISessionService session,
    IMfaService mfaService
) : IRequestHandler<RemoveMfaAuthenticatorCommand, ApiResponse>
{
    public async Task<ApiResponse> Handle(RemoveMfaAuthenticatorCommand request, CancellationToken cancellationToken)
    {
        var userId = session.GetUserClaims().UserId;

        var result = await mfaService.RemoveMfaFromUserAsync(userId, cancellationToken);
        return result
            ? new ApiResponse(ApiResponseTypes.Success)
            : new ApiResponse(ApiResponseTypes.Error, "Failed to remove MFA authenticator");
    }
}