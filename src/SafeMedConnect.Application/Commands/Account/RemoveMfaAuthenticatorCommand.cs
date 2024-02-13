using MediatR;
using SafeMedConnect.Domain.Interfaces.Repositories;
using SafeMedConnect.Domain.Interfaces.Services;
using SafeMedConnect.Domain.Responses;

namespace SafeMedConnect.Application.Commands.Account;

public sealed record RemoveMfaAuthenticatorCommand : IRequest<ResponseWrapper>;

public class RemoveMfaAuthenticatorCommandHandler(
    ISessionService session,
    IMfaRepository repository
) : IRequestHandler<RemoveMfaAuthenticatorCommand, ResponseWrapper>
{
    public async Task<ResponseWrapper> Handle(RemoveMfaAuthenticatorCommand request, CancellationToken cancellationToken)
    {
        var userId = session.GetUserClaims().UserId;

        var result = await repository.DeactivateMfaAsync(userId, cancellationToken);
        return result
            ? new ResponseWrapper(ResponseTypes.Success)
            : new ResponseWrapper(ResponseTypes.Error, "Failed to remove MFA authenticator");
    }
}