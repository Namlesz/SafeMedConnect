using FluentValidation;
using MediatR;
using SafeMedConnect.Application.Dto;
using SafeMedConnect.Domain.Interfaces.Services;
using SafeMedConnect.Domain.Responses;

namespace SafeMedConnect.Application.Commands.Share;

public sealed record ShareDataCommand(int MinutesToExpire) : IRequest<ResponseWrapper<TokenResponseDto>>;

public class ShareDataCommandHandler(ISessionService sessionService, ITokenService tokenService)
    : IRequestHandler<ShareDataCommand, ResponseWrapper<TokenResponseDto>>
{
    public Task<ResponseWrapper<TokenResponseDto>> Handle(ShareDataCommand request, CancellationToken cancellationToken)
    {
        var userId = sessionService.GetUserClaims().UserId;
        var token = tokenService.GenerateDataShareToken(request.MinutesToExpire, userId, cancellationToken);
        return Task.FromResult(new ResponseWrapper<TokenResponseDto>(ResponseTypes.Success, new TokenResponseDto(token)));
    }
}

public sealed class ShareDataCommandValidator : AbstractValidator<ShareDataCommand>
{
    public ShareDataCommandValidator()
    {
        RuleFor(x => x.MinutesToExpire).GreaterThan(0);
    }
}