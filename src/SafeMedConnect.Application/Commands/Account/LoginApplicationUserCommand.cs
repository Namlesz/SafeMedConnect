using FluentValidation;
using MediatR;
using OtpNet;
using SafeMedConnect.Application.Dto;
using SafeMedConnect.Domain.Interfaces.Repositories;
using SafeMedConnect.Domain.Interfaces.Services;
using SafeMedConnect.Domain.Responses;

namespace SafeMedConnect.Application.Commands.Account;

public sealed record LoginApplicationUserCommand(string Email, string Password, string? MfaCode = null)
    : IRequest<ResponseWrapper<TokenResponseDto>>;

internal sealed class LoginApplicationUserCommandHandler(
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
            return new ResponseWrapper<TokenResponseDto>(ResponseTypes.Forbidden, "Invalid login or password");
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

            if (!IsCodeValid(request.MfaCode, secret))
            {
                return new ResponseWrapper<TokenResponseDto>(ResponseTypes.InvalidRequest, "Invalid MFA code");
            }
        }

        var token = tokenService.GenerateJwtToken(user, cancellationToken);
        return new ResponseWrapper<TokenResponseDto>(ResponseTypes.Success, data: new TokenResponseDto(token));
    }

    // TODO: Move this to a service
    private static bool IsCodeValid(string code, string secret)
    {
        var secretKey = Base32Encoding.ToBytes(secret);
        var totp = new Totp(secretKey);
        return totp.VerifyTotp(code, out _);
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