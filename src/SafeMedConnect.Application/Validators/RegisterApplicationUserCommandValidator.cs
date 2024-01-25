using FluentValidation;
using SafeMedConnect.Application.Commands.Account;

namespace SafeMedConnect.Application.Validators;

public sealed class RegisterApplicationUserCommandValidator : AbstractValidator<RegisterApplicationUserCommand>
{
    public RegisterApplicationUserCommandValidator()
    {
        RuleFor(x => x.Login)
            .NotEmpty();

        RuleFor(x => x.Password)
            .NotEmpty();

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();
    }
}