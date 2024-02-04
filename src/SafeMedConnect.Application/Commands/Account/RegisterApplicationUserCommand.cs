using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using SafeMedConnect.Domain.Entities;
using SafeMedConnect.Domain.Interfaces.Repositories;
using SafeMedConnect.Domain.Responses;

namespace SafeMedConnect.Application.Commands.Account;

public sealed record RegisterApplicationUserCommand(string Password, string Email)
    : IRequest<ResponseWrapper>;

internal sealed class RegisterApplicationUserCommandHandler(
    IApplicationUserRepository repository,
    ILogger<RegisterApplicationUserCommandHandler> logger
) : IRequestHandler<RegisterApplicationUserCommand, ResponseWrapper>
{
    public async Task<ResponseWrapper> Handle(RegisterApplicationUserCommand request, CancellationToken cancellationToken)
    {
        var userExists = await repository.GetUserAsync(request.Email, cancellationToken);
        if (userExists is not null)
        {
            logger.LogError("User with login {Email} already exists", request.Email);
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

public sealed class RegisterApplicationUserCommandValidator : AbstractValidator<RegisterApplicationUserCommand>
{
    public RegisterApplicationUserCommandValidator()
    {
        RuleFor(x => x.Password)
            .NotEmpty();

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();
    }
}