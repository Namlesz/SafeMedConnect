using AutoMapper;
using MediatR;
using SafeMedConnect.Application.Dto.Measurements;
using SafeMedConnect.Domain.Abstract.Repositories;
using SafeMedConnect.Domain.Abstract.Services;
using SafeMedConnect.Domain.Entities;
using SafeMedConnect.Domain.Enums;
using SafeMedConnect.Domain.Models;
using static SafeMedConnect.Domain.ClaimTypes.SharedDataClaimTypes;

namespace SafeMedConnect.Application.Queries.Share;

public record GetSharedTemperatureQuery : IRequest<ApiResponse<List<TemperatureDto>>>;

public class GetSharedTemperatureQueryHandler(
    ISessionService session,
    IMapper mapper,
    IMeasurementRepository<TemperatureMeasurementEntity> temperatureRepository
) : IRequestHandler<GetSharedTemperatureQuery, ApiResponse<List<TemperatureDto>>>
{
    public async Task<ApiResponse<List<TemperatureDto>>> Handle(
        GetSharedTemperatureQuery request,
        CancellationToken cancellationToken
    )
    {
        var guestClaims = session.GetGuestClaims();
        if (guestClaims.DataShareClaims is null or { Count: 0 })
        {
            return new ApiResponse<List<TemperatureDto>>(ApiResponseTypes.InvalidRequest, "No data to share");
        }

        guestClaims.DataShareClaims.TryGetValue(ShareTemperatureMeasurement, out var shareBloodPressureMeasurement);
        if (!shareBloodPressureMeasurement)
        {
            return new ApiResponse<List<TemperatureDto>>(ApiResponseTypes.Forbidden);
        }

        var temperatureMeasurements = await temperatureRepository.GetAsync(guestClaims.UserId, cancellationToken);
        if (temperatureMeasurements is null)
        {
            return new ApiResponse<List<TemperatureDto>>(ApiResponseTypes.NotFound);
        }

        var result = mapper.Map<List<TemperatureDto>>(temperatureMeasurements.Measurements);
        return new ApiResponse<List<TemperatureDto>>(ApiResponseTypes.Success, result);
    }
}