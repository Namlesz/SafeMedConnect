using AutoMapper;
using MediatR;
using SafeMedConnect.Application.Dto;
using SafeMedConnect.Application.Dto.Measurements;
using SafeMedConnect.Domain.Entities;
using SafeMedConnect.Domain.Interfaces.Repositories;
using SafeMedConnect.Domain.Interfaces.Services;
using SafeMedConnect.Domain.Models;
using SafeMedConnect.Domain.Responses;
using static SafeMedConnect.Domain.ClaimTypes.DataShareClaimTypes;

namespace SafeMedConnect.Application.Queries.Share;

public sealed record GetSharedDataQuery : IRequest<ResponseWrapper<SharedDataDto>>;

public class GetSharedDataQueryHandler(
    ISessionService session,
    IMapper mapper,
    IUserRepository userRepository,
    IMeasurementRepository<HeartRateMeasurementEntity> heartRateRepository,
    IMeasurementRepository<BloodPressureMeasurementEntity> bloodPressureRepository,
    IMeasurementRepository<TemperatureMeasurementEntity> temperatureRepository,
    IMeasurementRepository<BloodSugarMeasurementEntity> bloodSugarRepository
) : IRequestHandler<GetSharedDataQuery, ResponseWrapper<SharedDataDto>>
{
    public async Task<ResponseWrapper<SharedDataDto>> Handle(GetSharedDataQuery request, CancellationToken cancellationToken)
    {
        var guestClaims = session.GetGuestClaims();
        if (guestClaims.DataShareClaims is null or { Count: 0 })
        {
            return new ResponseWrapper<SharedDataDto>(ResponseTypes.InvalidRequest, message: "No data to share");
        }

        var result = await GetSharedDataAsync(guestClaims, cancellationToken);
        return new ResponseWrapper<SharedDataDto>(ResponseTypes.Success, result);
    }

    private async Task<SharedDataDto> GetSharedDataAsync(
        GuestClaims guestClaims,
        CancellationToken cancellationToken
    )
    {
        var userId = guestClaims.UserId;
        var dataToShare = new SharedDataDto();

        guestClaims.DataShareClaims.TryGetValue(ShareSensitiveData, out var shareSensitiveData);
        if (shareSensitiveData)
        {
            var user = await userRepository.GetUserAsync(userId, cancellationToken);
            dataToShare.UserInformation = mapper.Map<UserDto?>(user);
        }

        guestClaims.DataShareClaims.TryGetValue(ShareBloodPressureMeasurement, out var shareBloodPressureMeasurement);
        if (shareBloodPressureMeasurement)
        {
            var bloodPressureMeasurements = await bloodPressureRepository.GetAsync(userId, cancellationToken);
            dataToShare.Measurements.BloodPressures =
                mapper.Map<List<BloodPressureDto>?>(bloodPressureMeasurements?.Measurements);
        }

        guestClaims.DataShareClaims.TryGetValue(ShareHeartRateMeasurement, out var shareHeartRateMeasurement);
        if (shareHeartRateMeasurement)
        {
            var heartRateMeasurements = await heartRateRepository.GetAsync(userId, cancellationToken);
            dataToShare.Measurements.HeartRates =
                mapper.Map<List<HeartRateDto>?>(heartRateMeasurements?.Measurements);
        }

        guestClaims.DataShareClaims.TryGetValue(ShareHeartRateMeasurement, out var shareTemperatureMeasurement);
        if (shareTemperatureMeasurement)
        {
            var temperatureMeasurements = await temperatureRepository.GetAsync(userId, cancellationToken);
            dataToShare.Measurements.Temperatures =
                mapper.Map<List<TemperatureDto>?>(temperatureMeasurements?.Measurements);
        }

        guestClaims.DataShareClaims.TryGetValue(ShareBloodSugarMeasurement, out var shareBloodSugarMeasurement);
        if (shareBloodSugarMeasurement)
        {
            var bloodSugarMeasurements = await bloodSugarRepository.GetAsync(userId, cancellationToken);
            dataToShare.Measurements.BloodSugars =
                mapper.Map<List<BloodSugarDto>?>(bloodSugarMeasurements?.Measurements);
        }

        return dataToShare;
    }
}