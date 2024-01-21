using MediatR;
using SafeMedConnect.Domain.Interfaces.Repositories;
using SafeMedConnect.Domain.Interfaces.Services;
using SafeMedConnect.Domain.Responses;

namespace SafeMedConnect.Application.Commands;

public sealed class LoginApplicationUserCommand : IRequest<ResponseWrapper<string>>
{
    public string Login { get; set; } = null!;
    public string Password { get; set; } = null!;
}

internal sealed class LoginApplicationUserCommandHandler(IApplicationUserRepository repository, ITokenService tokenService)
    : IRequestHandler<LoginApplicationUserCommand, ResponseWrapper<string>>
{
    public async Task<ResponseWrapper<string>> Handle(LoginApplicationUserCommand request, CancellationToken cancellationToken)
    {
        var user = await repository.GetUserAsync(request.Login, cancellationToken);
        if (user is null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        {
            return new ResponseWrapper<string>(ResponseTypes.Forbidden, "Invalid login or password");
        }

        var token = tokenService.GenerateJwtToken(user);
        return new ResponseWrapper<string>(ResponseTypes.Success, data: token);
    }
}