using MediatR;
using Microsoft.Extensions.Logging;
using SafeMedConnect.Domain.Entities;
using SafeMedConnect.Domain.Interfaces.Repositories;

namespace SafeMedConnect.Application.Commands;

public sealed class RegisterApplicationUserCommand : IRequest<bool>
{
    public string Login { get; set; } = null!;
    public string Password { get; set; } = null!;
}

// TODO: Standardize response
public class RegisterApplicationUserCommandHandler(
    IApplicationUserRepository repository,
    ILogger<RegisterApplicationUserCommandHandler> logger
) : IRequestHandler<RegisterApplicationUserCommand, bool>
{
    public async Task<bool> Handle(RegisterApplicationUserCommand request, CancellationToken cancellationToken)
    {
        var userExists = await repository.GetUserAsync(request.Login, cancellationToken);
        if (userExists != null)
        {
            logger.LogError("User with login {Login} already exists", request.Login);
            return false;
        }

        var hash = BCrypt.Net.BCrypt.HashPassword(request.Password);
        if (hash == null)
        {
            logger.LogError("Failed to hash password");
            return false;
        }

        var user = new ApplicationUserEntity
        {
            Login = request.Login,
            PasswordHash = hash
        };

        return await repository.RegisterUserAsync(user, cancellationToken);
    }
}