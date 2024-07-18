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

public sealed record GetSharedBloodPressureQuery : IRequest<ApiResponse<List<BloodPressureDto>>>;

public class GetSharedBloodPressureQueryHandler(
    ISessionService session,
    IMapper mapper,
    IMeasurementRepository<BloodPressureMeasurementEntity> bloodPressureRepository
) : IRequestHandler<GetSharedBloodPressureQuery, ApiResponse<List<BloodPressureDto>>>
{
    public async Task<ApiResponse<List<BloodPressureDto>>> Handle(
        GetSharedBloodPressureQuery request,
        CancellationToken cancellationToken
    )
    {
        var guestClaims = session.GetGuestClaims();
        if (guestClaims.DataShareClaims is null or { Count: 0 })
        {
            return new ApiResponse<List<BloodPressureDto>>(ApiResponseTypes.InvalidRequest, "No data to share");
        }

        guestClaims.DataShareClaims.TryGetValue(ShareBloodPressureMeasurement, out var shareBloodPressureMeasurement);
        if (!shareBloodPressureMeasurement)
        {
            return new ApiResponse<List<BloodPressureDto>>(ApiResponseTypes.Forbidden);
        }

        var bloodPressureMeasurements = await bloodPressureRepository.GetAsync(guestClaims.UserId, cancellationToken);
        if (bloodPressureMeasurements is null)
        {
            return new ApiResponse<List<BloodPressureDto>>(ApiResponseTypes.NotFound);
        }

        var result = mapper.Map<List<BloodPressureDto>>(bloodPressureMeasurements.Measurements);
        return new ApiResponse<List<BloodPressureDto>>(ApiResponseTypes.Success, result);
    }
}