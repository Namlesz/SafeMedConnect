using FluentValidation;
using MediatR;
using SafeMedConnect.Application.Dto;
using SafeMedConnect.Domain.Abstract.Repositories;
using SafeMedConnect.Domain.Abstract.Services;
using SafeMedConnect.Domain.Enums;
using SafeMedConnect.Domain.Models;

namespace SafeMedConnect.Application.Commands.Account;

public sealed record LoginApplicationUserCommand(string Email, string Password, string? MfaCode = null)
    : IRequest<ApiResponse<TokenResponseDto>>;

internal sealed class LoginApplicationUserCommandHandler(
    IMfaService mfaService,
    IApplicationUserRepository repository,
    ITokenService tokenService
)
    : IRequestHandler<LoginApplicationUserCommand, ApiResponse<TokenResponseDto>>
{
    public async Task<ApiResponse<TokenResponseDto>> Handle(
        LoginApplicationUserCommand request,
        CancellationToken cancellationToken
    )
    {
        var user = await repository.GetUserAsync(request.Email, cancellationToken);
        if (user is null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        {
            return new ApiResponse<TokenResponseDto>(ApiResponseTypes.Unauthorized, "Invalid login or password");
        }

        if (user.MfaEnabled)
        {
            if (request.MfaCode is null)
            {
                return new ApiResponse<TokenResponseDto>(ApiResponseTypes.InvalidRequest, "MFA code required");
            }

            var secret = user.MfaSecret;
            if (secret is null)
            {
                return new ApiResponse<TokenResponseDto>(ApiResponseTypes.Error, "MFA error occurred");
            }

            if (!mfaService.IsCodeValid(request.MfaCode, secret))
            {
                return new ApiResponse<TokenResponseDto>(ApiResponseTypes.Forbidden, "Invalid MFA code");
            }
        }

        var token = tokenService.GenerateJwtToken(user, cancellationToken);
        return new ApiResponse<TokenResponseDto>(ApiResponseTypes.Success, new TokenResponseDto(token));
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