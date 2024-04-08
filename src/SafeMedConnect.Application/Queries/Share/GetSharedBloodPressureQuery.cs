using AutoMapper;
using MediatR;
using SafeMedConnect.Application.Dto.Measurements;
using SafeMedConnect.Domain.Entities;
using SafeMedConnect.Domain.Interfaces.Repositories;
using SafeMedConnect.Domain.Interfaces.Services;
using SafeMedConnect.Domain.Responses;
using static SafeMedConnect.Domain.ClaimTypes.DataShareClaimTypes;

namespace SafeMedConnect.Application.Queries.Share;

public sealed record GetSharedBloodPressureQuery : IRequest<ResponseWrapper<List<BloodPressureDto>>>;

public class GetSharedBloodPressureQueryHandler(
    ISessionService session,
    IMapper mapper,
    IMeasurementRepository<BloodPressureMeasurementEntity> bloodPressureRepository
) : IRequestHandler<GetSharedBloodPressureQuery, ResponseWrapper<List<BloodPressureDto>>>
{
    public async Task<ResponseWrapper<List<BloodPressureDto>>> Handle(
        GetSharedBloodPressureQuery request,
        CancellationToken cancellationToken
    )
    {
        var guestClaims = session.GetGuestClaims();
        if (guestClaims.DataShareClaims is null or { Count: 0 })
        {
            return new ResponseWrapper<List<BloodPressureDto>>(ResponseTypes.InvalidRequest, message: "No data to share");
        }

        guestClaims.DataShareClaims.TryGetValue(ShareBloodPressureMeasurement, out var shareBloodPressureMeasurement);
        if (!shareBloodPressureMeasurement)
        {
            return new ResponseWrapper<List<BloodPressureDto>>(ResponseTypes.Forbidden);
        }

        var bloodPressureMeasurements = await bloodPressureRepository.GetAsync(guestClaims.UserId, cancellationToken);
        if (bloodPressureMeasurements is null)
        {
            return new ResponseWrapper<List<BloodPressureDto>>(ResponseTypes.NotFound);
        }

        var result = mapper.Map<List<BloodPressureDto>>(bloodPressureMeasurements.Measurements);
        return new ResponseWrapper<List<BloodPressureDto>>(ResponseTypes.Success, result);
    }
}