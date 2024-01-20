using MediatR;
using SafeMedConnect.Domain.Responses;

namespace SafeMedConnect.Application.Commands;

public sealed class LoginApplicationUserCommand : IRequest<ResponseWrapper<object>>
{
}

public class LoginApplicationUserCommandHandler : IRequestHandler<LoginApplicationUserCommand, ResponseWrapper<object>>
{
    public Task<ResponseWrapper<object>> Handle(LoginApplicationUserCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}