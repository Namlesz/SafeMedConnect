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

public record GetSharedHeartRateQuery : IRequest<ApiResponse<List<HeartRateDto>>>;

public class GetSharedHeartRateQueryHandler(
    ISessionService session,
    IMapper mapper,
    IMeasurementRepository<HeartRateMeasurementEntity> heartRateRepository
) : IRequestHandler<GetSharedHeartRateQuery, ApiResponse<List<HeartRateDto>>>
{
    public async Task<ApiResponse<List<HeartRateDto>>> Handle(
        GetSharedHeartRateQuery request,
        CancellationToken cancellationToken
    )
    {
        var guestClaims = session.GetGuestClaims();
        if (guestClaims.DataShareClaims is null or { Count: 0 })
        {
            return new ApiResponse<List<HeartRateDto>>(ApiResponseTypes.InvalidRequest, "No data to share");
        }

        guestClaims.DataShareClaims.TryGetValue(ShareHeartRateMeasurement, out var shareBloodPressureMeasurement);
        if (!shareBloodPressureMeasurement)
        {
            return new ApiResponse<List<HeartRateDto>>(ApiResponseTypes.Forbidden);
        }

        var heartRateMeasurements = await heartRateRepository.GetAsync(guestClaims.UserId, cancellationToken);
        if (heartRateMeasurements is null)
        {
            return new ApiResponse<List<HeartRateDto>>(ApiResponseTypes.NotFound);
        }

        var result = mapper.Map<List<HeartRateDto>>(heartRateMeasurements.Measurements);
        return new ApiResponse<List<HeartRateDto>>(ApiResponseTypes.Success, result);
    }
}