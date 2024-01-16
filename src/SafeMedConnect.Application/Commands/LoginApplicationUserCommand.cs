using MediatR;

namespace SafeMedConnect.Application.Commands;

public sealed class LoginApplicationUserCommand : IRequest<object>
{

}

public class LoginApplicationUserCommandHandler : IRequestHandler<LoginApplicationUserCommand, object>
{
    public Task<object> Handle(LoginApplicationUserCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}