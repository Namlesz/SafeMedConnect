using FluentValidation;
using SafeMedConnect.Application.Commands;

namespace SafeMedConnect.Application.Validators;

public sealed class RegisterApplicationUserCommandValidator : AbstractValidator<RegisterApplicationUserCommand>
{
    public RegisterApplicationUserCommandValidator()
    {
        RuleFor(x => x.Login)
            .NotEmpty()
            .WithMessage("Login cannot be empty");

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Password cannot be empty");
    }
}