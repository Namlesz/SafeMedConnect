using FluentValidation;
using SafeMedConnect.Application.Commands;

namespace SafeMedConnect.Application.Validators;

public sealed class LoginApplicationUserCommandValidator : AbstractValidator<LoginApplicationUserCommand>
{
    public LoginApplicationUserCommandValidator()
    {
        RuleFor(x => x.Login)
            .NotEmpty();

        RuleFor(x => x.Password)
            .NotEmpty();
    }
}