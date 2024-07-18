using System.Security.Claims;
using FluentValidation;
using MediatR;
using SafeMedConnect.Application.Dto;
using SafeMedConnect.Domain.Abstract.Services;
using SafeMedConnect.Domain.ClaimTypes;
using SafeMedConnect.Domain.Enums;
using SafeMedConnect.Domain.Models;

namespace SafeMedConnect.Application.Commands.Share;

public sealed record ShareDataCommand(
    int MinutesToExpire,
    bool ShareSensitiveData = false,
    bool ShareBloodPressureMeasurement = false,
    bool ShareHeartRateMeasurement = false,
    bool ShareTemperatureMeasurement = false,
    bool ShareBloodSugarMeasurement = false
) : IRequest<ApiResponse<TokenResponseDto>>;

public class ShareDataCommandHandler(ISessionService sessionService, ITokenService tokenService)
    : IRequestHandler<ShareDataCommand, ApiResponse<TokenResponseDto>>
{
    public Task<ApiResponse<TokenResponseDto>> Handle(ShareDataCommand request, CancellationToken cancellationToken)
    {
        var userId = sessionService.GetUserClaims().UserId;
        var claims = new List<Claim>
        {
            new(SharedDataClaimTypes.ShareSensitiveData,
                request.ShareSensitiveData.ToString(),
                ClaimValueTypes.Boolean),
            new(SharedDataClaimTypes.ShareBloodPressureMeasurement,
                request.ShareBloodPressureMeasurement.ToString(),
                ClaimValueTypes.Boolean),
            new(SharedDataClaimTypes.ShareHeartRateMeasurement,
                request.ShareHeartRateMeasurement.ToString(),
                ClaimValueTypes.Boolean),
            new(SharedDataClaimTypes.ShareTemperatureMeasurement,
                request.ShareTemperatureMeasurement.ToString(),
                ClaimValueTypes.Boolean),
            new(SharedDataClaimTypes.ShareBloodSugarMeasurement,
                request.ShareBloodSugarMeasurement.ToString(),
                ClaimValueTypes.Boolean)
        };

        var token = tokenService.GenerateShareToken(request.MinutesToExpire, userId, claims, cancellationToken);
        return Task.FromResult(new ApiResponse<TokenResponseDto>(ApiResponseTypes.Success, new TokenResponseDto(token)));
    }
}

public sealed class ShareDataCommandValidator : AbstractValidator<ShareDataCommand>
{
    public ShareDataCommandValidator()
    {
        RuleFor(x => x.MinutesToExpire).GreaterThan(0);
    }
}