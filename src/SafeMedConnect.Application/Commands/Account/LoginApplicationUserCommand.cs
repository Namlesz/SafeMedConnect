using FluentValidation;
using MediatR;
using SafeMedConnect.Application.Dto;
using SafeMedConnect.Domain.Interfaces.Repositories;
using SafeMedConnect.Domain.Interfaces.Services;
using SafeMedConnect.Domain.Responses;

namespace SafeMedConnect.Application.Commands.Account;

public sealed record LoginApplicationUserCommand(string Email, string Password, string? MfaCode = null)
    : IRequest<ResponseWrapper<TokenResponseDto>>;

internal sealed class LoginApplicationUserCommandHandler(
    IMfaService mfaService,
    IApplicationUserRepository repository,
    ITokenService tokenService
)
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
            return new ResponseWrapper<TokenResponseDto>(ResponseTypes.Unauthorized, "Invalid login or password");
        }

        if (user.MfaEnabled)
        {
            if (request.MfaCode is null)
            {
                return new ResponseWrapper<TokenResponseDto>(ResponseTypes.InvalidRequest, "MFA code required");
            }

            var secret = user.MfaSecret;
            if (secret is null)
            {
                return new ResponseWrapper<TokenResponseDto>(ResponseTypes.Error, "MFA not configured");
            }

            if (!mfaService.IsCodeValid(request.MfaCode, secret))
            {
                return new ResponseWrapper<TokenResponseDto>(ResponseTypes.Forbidden, "Invalid MFA code");
            }
        }

        var token = tokenService.GenerateJwtToken(user, cancellationToken);
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