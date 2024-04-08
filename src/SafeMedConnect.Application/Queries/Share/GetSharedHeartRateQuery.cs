using AutoMapper;
using MediatR;
using SafeMedConnect.Application.Dto.Measurements;
using SafeMedConnect.Domain.Entities;
using SafeMedConnect.Domain.Interfaces.Repositories;
using SafeMedConnect.Domain.Interfaces.Services;
using SafeMedConnect.Domain.Responses;
using static SafeMedConnect.Domain.ClaimTypes.DataShareClaimTypes;

namespace SafeMedConnect.Application.Queries.Share;

public record GetSharedHeartRateQuery : IRequest<ResponseWrapper<List<HeartRateDto>>>;

public class GetSharedHeartRateQueryHandler(
    ISessionService session,
    IMapper mapper,
    IMeasurementRepository<HeartRateMeasurementEntity> heartRateRepository
) : IRequestHandler<GetSharedHeartRateQuery, ResponseWrapper<List<HeartRateDto>>>
{
    public async Task<ResponseWrapper<List<HeartRateDto>>> Handle(
        GetSharedHeartRateQuery request,
        CancellationToken cancellationToken
    )
    {
        var guestClaims = session.GetGuestClaims();
        if (guestClaims.DataShareClaims is null or { Count: 0 })
        {
            return new ResponseWrapper<List<HeartRateDto>>(ResponseTypes.InvalidRequest, message: "No data to share");
        }

        guestClaims.DataShareClaims.TryGetValue(ShareBloodPressureMeasurement, out var shareBloodPressureMeasurement);
        if (!shareBloodPressureMeasurement)
        {
            return new ResponseWrapper<List<HeartRateDto>>(ResponseTypes.Forbidden);
        }

        var heartRateMeasurements = await heartRateRepository.GetAsync(guestClaims.UserId, cancellationToken);
        if (heartRateMeasurements is null)
        {
            return new ResponseWrapper<List<HeartRateDto>>(ResponseTypes.NotFound);
        }

        var result = mapper.Map<List<HeartRateDto>>(heartRateMeasurements.Measurements);
        return new ResponseWrapper<List<HeartRateDto>>(ResponseTypes.Success, result);
    }
}