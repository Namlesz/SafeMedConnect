using AutoMapper;
using MediatR;
using SafeMedConnect.Application.Dto.Measurements;
using SafeMedConnect.Domain.Entities;
using SafeMedConnect.Domain.Interfaces.Repositories;
using SafeMedConnect.Domain.Interfaces.Services;
using SafeMedConnect.Domain.Responses;
using static SafeMedConnect.Domain.ClaimTypes.DataShareClaimTypes;

namespace SafeMedConnect.Application.Queries.Share;

public record GetSharedTemperatureQuery : IRequest<ResponseWrapper<List<TemperatureDto>>>;

public class GetSharedTemperatureQueryHandler(
    ISessionService session,
    IMapper mapper,
    IMeasurementRepository<TemperatureMeasurementEntity> temperatureRepository
) : IRequestHandler<GetSharedTemperatureQuery, ResponseWrapper<List<TemperatureDto>>>
{
    public async Task<ResponseWrapper<List<TemperatureDto>>> Handle(
        GetSharedTemperatureQuery request,
        CancellationToken cancellationToken
    )
    {
        var guestClaims = session.GetGuestClaims();
        if (guestClaims.DataShareClaims is null or { Count: 0 })
        {
            return new ResponseWrapper<List<TemperatureDto>>(ResponseTypes.InvalidRequest, message: "No data to share");
        }

        guestClaims.DataShareClaims.TryGetValue(ShareTemperatureMeasurement, out var shareBloodPressureMeasurement);
        if (!shareBloodPressureMeasurement)
        {
            return new ResponseWrapper<List<TemperatureDto>>(ResponseTypes.Forbidden);
        }

        var temperatureMeasurements = await temperatureRepository.GetAsync(guestClaims.UserId, cancellationToken);
        if (temperatureMeasurements is null)
        {
            return new ResponseWrapper<List<TemperatureDto>>(ResponseTypes.NotFound);
        }

        var result = mapper.Map<List<TemperatureDto>>(temperatureMeasurements.Measurements);
        return new ResponseWrapper<List<TemperatureDto>>(ResponseTypes.Success, result);
    }
}