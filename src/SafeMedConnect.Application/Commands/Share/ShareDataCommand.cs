using FluentValidation;
using MediatR;
using SafeMedConnect.Application.Dto;
using SafeMedConnect.Domain.ClaimTypes;
using SafeMedConnect.Domain.Interfaces.Services;
using SafeMedConnect.Domain.Responses;
using System.Security.Claims;

namespace SafeMedConnect.Application.Commands.Share;

public sealed record ShareDataCommand(
    int MinutesToExpire,
    bool ShareSensitiveData = false,
    bool ShareBloodPressureMeasurement = false,
    bool ShareHeartRateMeasurement = false
) : IRequest<ResponseWrapper<TokenResponseDto>>;

public class ShareDataCommandHandler(ISessionService sessionService, ITokenService tokenService)
    : IRequestHandler<ShareDataCommand, ResponseWrapper<TokenResponseDto>>
{
    public Task<ResponseWrapper<TokenResponseDto>> Handle(ShareDataCommand request, CancellationToken cancellationToken)
    {
        var userId = sessionService.GetUserClaims().UserId;
        var claims = new List<Claim>
        {
            new(DataShareClaimTypes.ShareSensitiveData,
                request.ShareSensitiveData.ToString(),
                ClaimValueTypes.Boolean),
            new(DataShareClaimTypes.ShareBloodPressureMeasurement,
                request.ShareBloodPressureMeasurement.ToString(),
                ClaimValueTypes.Boolean),
            new(DataShareClaimTypes.ShareHeartRateMeasurement,
                request.ShareHeartRateMeasurement.ToString(),
                ClaimValueTypes.Boolean)
        };

        var token = tokenService.GenerateShareToken(request.MinutesToExpire, userId, claims, cancellationToken);
        return Task.FromResult(new ResponseWrapper<TokenResponseDto>(ResponseTypes.Success, new TokenResponseDto(token)));
    }
}

public sealed class ShareDataCommandValidator : AbstractValidator<ShareDataCommand>
{
    public ShareDataCommandValidator()
    {
        RuleFor(x => x.MinutesToExpire).GreaterThan(0);
        RuleFor(x => x.ShareSensitiveData).NotEmpty();
        RuleFor(x => x.ShareBloodPressureMeasurement).NotEmpty();
        RuleFor(x => x.ShareHeartRateMeasurement).NotEmpty();
    }
}