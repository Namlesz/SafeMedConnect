using MediatR;
using SafeMedConnect.Application.Factories;
using SafeMedConnect.Domain.Abstract.Repositories;
using SafeMedConnect.Domain.Abstract.Services;
using SafeMedConnect.Domain.Entities;
using SafeMedConnect.Domain.Models;

namespace SafeMedConnect.Application.Queries.BloodPressure;

public sealed record GetBloodPressureQuery : IRequest<ApiResponse<List<BloodPressureMeasurementEntity>>>;

public class GetBloodPressureQueryHandler(
    ISessionService session,
    IMeasurementRepository<BloodPressureMeasurementEntity> repository
) : IRequestHandler<GetBloodPressureQuery, ApiResponse<List<BloodPressureMeasurementEntity>>>
{
    public Task<ApiResponse<List<BloodPressureMeasurementEntity>>> Handle(
        GetBloodPressureQuery request,
        CancellationToken cancellationToken
    )
    {
        var userId = session.GetUserClaims().UserId;

        return new MeasurementFactory<BloodPressureMeasurementEntity>(repository, userId)
            .GetMeasurementsAsync(cancellationToken);
    }
}