using MediatR;
using SafeMedConnect.Domain.ClaimTypes;
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
        if (guestClaims.DataShareClaims is null or { Count: 0 })
        {
            return new ResponseWrapper<object>(ResponseTypes.InvalidRequest, message: "No data to share");
        }

        if (guestClaims.DataShareClaims[DataShareClaimTypes.ShareSensitiveData])
        {
            // Add sensitive data to result
        }

        if (guestClaims.DataShareClaims[DataShareClaimTypes.ShareBloodPressureMeasurement])
        {
            // Add blood data to result
        }

        if (guestClaims.DataShareClaims[DataShareClaimTypes.ShareHeartRateMeasurement])
        {
            // Add heart rate to result
        }

        return new ResponseWrapper<object>(
            ResponseTypes.Success,
            new
            {
                guestClaims
            }
        );
    }
}