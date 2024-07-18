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

public record GetSharedBloodSugarQuery : IRequest<ApiResponse<List<BloodSugarDto>>>;

public class GetSharedBloodSugarQueryHandler(
    ISessionService session,
    IMapper mapper,
    IMeasurementRepository<BloodSugarMeasurementEntity> bloodSugarRepository
) : IRequestHandler<GetSharedBloodSugarQuery, ApiResponse<List<BloodSugarDto>>>
{
    public async Task<ApiResponse<List<BloodSugarDto>>> Handle(
        GetSharedBloodSugarQuery request,
        CancellationToken cancellationToken
    )
    {
        var guestClaims = session.GetGuestClaims();
        if (guestClaims.DataShareClaims is null or { Count: 0 })
        {
            return new ApiResponse<List<BloodSugarDto>>(ApiResponseTypes.InvalidRequest, "No data to share");
        }

        guestClaims.DataShareClaims.TryGetValue(ShareBloodSugarMeasurement, out var shareBloodPressureMeasurement);
        if (!shareBloodPressureMeasurement)
        {
            return new ApiResponse<List<BloodSugarDto>>(ApiResponseTypes.Forbidden);
        }

        var bloodSugarMeasurements = await bloodSugarRepository.GetAsync(guestClaims.UserId, cancellationToken);
        if (bloodSugarMeasurements is null)
        {
            return new ApiResponse<List<BloodSugarDto>>(ApiResponseTypes.NotFound);
        }

        var result = mapper.Map<List<BloodSugarDto>>(bloodSugarMeasurements.Measurements);
        return new ApiResponse<List<BloodSugarDto>>(ApiResponseTypes.Success, result);
    }
}