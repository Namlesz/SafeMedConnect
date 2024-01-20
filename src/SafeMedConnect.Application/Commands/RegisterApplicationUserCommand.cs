using MediatR;
using Microsoft.Extensions.Logging;
using SafeMedConnect.Domain.Entities;
using SafeMedConnect.Domain.Interfaces.Repositories;
using SafeMedConnect.Domain.Responses;

namespace SafeMedConnect.Application.Commands;

public sealed class RegisterApplicationUserCommand : IRequest<ResponseWrapper>
{
    public string Login { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string Email { get; set; } = null!;
}

internal sealed class RegisterApplicationUserCommandHandler(
    IApplicationUserRepository repository,
    ILogger<RegisterApplicationUserCommandHandler> logger
) : IRequestHandler<RegisterApplicationUserCommand, ResponseWrapper>
{
    public async Task<ResponseWrapper> Handle(RegisterApplicationUserCommand request, CancellationToken cancellationToken)
    {
        var userExists = await repository.GetUserAsync(request.Login, cancellationToken);
        if (userExists is not null)
        {
            logger.LogError("User with login {Login} already exists", request.Login);
            return new ResponseWrapper(ResponseTypes.Conflict, "User already exists");
        }

        var hash = BCrypt.Net.BCrypt.HashPassword(request.Password);
        if (hash is null)
        {
            logger.LogError("Failed to hash password");
            return new ResponseWrapper(ResponseTypes.Error, "An error occurred while registering the user");
        }

        var user = new ApplicationUserEntity
        {
            Login = request.Login,
            PasswordHash = hash,
            Email = request.Email
        };

        var isSaved = await repository.RegisterUserAsync(user, cancellationToken);
        if (!isSaved)
        {
            logger.LogError("Failed to save user");
            return new ResponseWrapper(ResponseTypes.Error, "An error occurred while registering the user");
        }

        return new ResponseWrapper(ResponseTypes.Success);
    }
}