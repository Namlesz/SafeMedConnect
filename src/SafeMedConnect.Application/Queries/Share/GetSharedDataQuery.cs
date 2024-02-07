using MediatR;
using SafeMedConnect.Domain.Interfaces.Services;
using SafeMedConnect.Domain.Responses;

namespace SafeMedConnect.Application.Queries.Share;

public sealed record GetSharedDataQuery : IRequest<ResponseWrapper<object>>;

public class GetSharedDataQueryHandler(
    ISessionService session
) : IRequestHandler<GetSharedDataQuery, ResponseWrapper<object>>
{
#pragma warning disable CS1998
    public async Task<ResponseWrapper<object>> Handle(GetSharedDataQuery request, CancellationToken cancellationToken)
    {
        var guestClaims = session.GetGuestClaims();
        return new ResponseWrapper<object>(
            ResponseTypes.Success,
            new
            {
                userId = guestClaims
            }
        );
    }
}