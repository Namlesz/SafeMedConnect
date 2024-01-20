using MediatR;
using SafeMedConnect.Domain.Interfaces.Repositories;
using SafeMedConnect.Domain.Responses;

namespace SafeMedConnect.Application.Commands;

public sealed class LoginApplicationUserCommand : IRequest<ResponseWrapper<object>>
{
    public string Login { get; set; } = null!;
    public string Password { get; set; } = null!;
}

internal sealed class LoginApplicationUserCommandHandler(IApplicationUserRepository repository)
    : IRequestHandler<LoginApplicationUserCommand, ResponseWrapper<object>>
{
    public async Task<ResponseWrapper<object>> Handle(LoginApplicationUserCommand request, CancellationToken cancellationToken)
    {
        var user = await repository.GetUserAsync(request.Login, cancellationToken);
        if (user is null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        {
            return new ResponseWrapper<object>(ResponseTypes.Forbidden, "Invalid login or password");
        }

        // TODO: Generate JWT token
        throw new NotImplementedException();
    }
}