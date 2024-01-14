using FluentValidation;
using SafeMedConnect.Application.Queries;

namespace SafeMedConnect.Application.Validators;

public sealed class GetAccountNameQueryValidator : AbstractValidator<GetAccountNameQuery>
{
    public GetAccountNameQueryValidator()
    {
        RuleFor(x => x.AccountId).GreaterThan(10);
    }
}