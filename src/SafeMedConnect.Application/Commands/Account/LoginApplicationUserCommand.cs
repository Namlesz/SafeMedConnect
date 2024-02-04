using FluentValidation;
using MediatR;
using SafeMedConnect.Application.Dto;
using SafeMedConnect.Domain.Interfaces.Repositories;
using SafeMedConnect.Domain.Interfaces.Services;
using SafeMedConnect.Domain.Responses;

namespace SafeMedConnect.Application.Commands.Account;

public sealed record LoginApplicationUserCommand(string Email, string Password)
    : IRequest<ResponseWrapper<TokenResponseDto>>;

internal sealed class LoginApplicationUserCommandHandler(IApplicationUserRepository repository, ITokenService tokenService)
    : IRequestHandler<LoginApplicationUserCommand, ResponseWrapper<TokenResponseDto>>
{
    public async Task<ResponseWrapper<TokenResponseDto>> Handle(
        LoginApplicationUserCommand request,
        CancellationToken cancellationToken
    )
    {
        var user = await repository.GetUserAsync(request.Email, cancellationToken);
        if (user is null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        {
            return new ResponseWrapper<TokenResponseDto>(ResponseTypes.Forbidden, "Invalid login or password");
        }

        var token = tokenService.GenerateJwtToken(user);
        return new ResponseWrapper<TokenResponseDto>(ResponseTypes.Success, data: new TokenResponseDto(token));
    }
}

public sealed class LoginApplicationUserCommandValidator : AbstractValidator<LoginApplicationUserCommand>
{
    public LoginApplicationUserCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.Password)
            .NotEmpty();
    }
}