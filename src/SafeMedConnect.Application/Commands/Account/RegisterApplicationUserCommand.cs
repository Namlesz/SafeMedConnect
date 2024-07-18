using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using SafeMedConnect.Domain.Abstract.Repositories;
using SafeMedConnect.Domain.Entities;
using SafeMedConnect.Domain.Enums;
using SafeMedConnect.Domain.Models;

namespace SafeMedConnect.Application.Commands.Account;

public sealed record RegisterApplicationUserCommand(string Password, string Email)
    : IRequest<ApiResponse>;

internal sealed class RegisterApplicationUserCommandHandler(
    IApplicationUserRepository repository,
    ILogger<RegisterApplicationUserCommandHandler> logger
) : IRequestHandler<RegisterApplicationUserCommand, ApiResponse>
{
    public async Task<ApiResponse> Handle(RegisterApplicationUserCommand request, CancellationToken cancellationToken)
    {
        var userExists = await repository.GetUserAsync(request.Email, cancellationToken);
        if (userExists is not null)
        {
            logger.LogError("User with login {Email} already exists", request.Email);
            return new ApiResponse(ApiResponseTypes.Conflict, "User already exists");
        }

        var hash = BCrypt.Net.BCrypt.HashPassword(request.Password);
        if (hash is null)
        {
            logger.LogError("Failed to hash password");
            return new ApiResponse(ApiResponseTypes.Error, "An error occurred while registering the user");
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
            return new ApiResponse(ApiResponseTypes.Error, "An error occurred while registering the user");
        }

        return new ApiResponse(ApiResponseTypes.Success);
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