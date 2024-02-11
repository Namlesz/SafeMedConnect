using AutoMapper;
using MediatR;
using SafeMedConnect.Application.Dto;
using SafeMedConnect.Domain.Entities;
using SafeMedConnect.Domain.Interfaces.Repositories;
using SafeMedConnect.Domain.Interfaces.Services;
using SafeMedConnect.Domain.Responses;
using static SafeMedConnect.Domain.ClaimTypes.DataShareClaimTypes;

namespace SafeMedConnect.Application.Queries.Share;

public sealed record GetSharedDataQuery : IRequest<ResponseWrapper<SharedDataDto>>;

public class GetSharedDataQueryHandler(
    ISessionService session,
    IMapper mapper,
    IUserRepository userRepository,
    IMeasurementRepository<HeartRateEntity, HeartRateMeasurementEntity> heartRateRepository,
    IMeasurementRepository<BloodPressureEntity, BloodPressureMeasurementEntity> bloodPressureRepository
) : IRequestHandler<GetSharedDataQuery, ResponseWrapper<SharedDataDto>>
{
    public async Task<ResponseWrapper<SharedDataDto>> Handle(GetSharedDataQuery request, CancellationToken cancellationToken)
    {
        var guestClaims = session.GetGuestClaims();
        if (guestClaims.DataShareClaims is null or { Count: 0 })
        {
            return new ResponseWrapper<SharedDataDto>(ResponseTypes.InvalidRequest, message: "No data to share");
        }

        var dataToShare = new SharedDataDto();
        var userId = guestClaims.UserId;

        guestClaims.DataShareClaims.TryGetValue(ShareSensitiveData, out var shareSensitiveData);
        if (shareSensitiveData)
        {
            var user = await userRepository.GetUserAsync(userId, cancellationToken);
            dataToShare.UserInformation = mapper.Map<UserDto>(user);
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
                mapper.Map<List<HeartRateDto>>(heartRateMeasurements?.Measurements);
        }

        return new ResponseWrapper<SharedDataDto>(ResponseTypes.Success, dataToShare);
    }
}