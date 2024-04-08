using AutoMapper;
using MediatR;
using SafeMedConnect.Application.Dto.Measurements;
using SafeMedConnect.Domain.Entities;
using SafeMedConnect.Domain.Interfaces.Repositories;
using SafeMedConnect.Domain.Interfaces.Services;
using SafeMedConnect.Domain.Responses;
using static SafeMedConnect.Domain.ClaimTypes.DataShareClaimTypes;

namespace SafeMedConnect.Application.Queries.Share;

public record GetSharedBloodSugarQuery : IRequest<ResponseWrapper<List<BloodSugarDto>>>;

public class GetSharedBloodSugarQueryHandler(
    ISessionService session,
    IMapper mapper,
    IMeasurementRepository<BloodSugarMeasurementEntity> bloodSugarRepository
) : IRequestHandler<GetSharedBloodSugarQuery, ResponseWrapper<List<BloodSugarDto>>>
{
    public async Task<ResponseWrapper<List<BloodSugarDto>>> Handle(
        GetSharedBloodSugarQuery request,
        CancellationToken cancellationToken
    )
    {
        var guestClaims = session.GetGuestClaims();
        if (guestClaims.DataShareClaims is null or { Count: 0 })
        {
            return new ResponseWrapper<List<BloodSugarDto>>(ResponseTypes.InvalidRequest, message: "No data to share");
        }

        guestClaims.DataShareClaims.TryGetValue(ShareBloodSugarMeasurement, out var shareBloodPressureMeasurement);
        if (!shareBloodPressureMeasurement)
        {
            return new ResponseWrapper<List<BloodSugarDto>>(ResponseTypes.Forbidden);
        }

        var bloodSugarMeasurements = await bloodSugarRepository.GetAsync(guestClaims.UserId, cancellationToken);
        if (bloodSugarMeasurements is null)
        {
            return new ResponseWrapper<List<BloodSugarDto>>(ResponseTypes.NotFound);
        }

        var result = mapper.Map<List<BloodSugarDto>>(bloodSugarMeasurements.Measurements);
        return new ResponseWrapper<List<BloodSugarDto>>(ResponseTypes.Success, result);
    }
}