using MediatR;
using SafeMedConnect.Application.Factories;
using SafeMedConnect.Domain.Abstract.Repositories;
using SafeMedConnect.Domain.Abstract.Services;
using SafeMedConnect.Domain.Entities;
using SafeMedConnect.Domain.Models;

namespace SafeMedConnect.Application.Queries.HeartRate;

public record GetHeartRateMeasurementsQuery : IRequest<ApiResponse<List<HeartRateMeasurementEntity>>>;

public class GetHeartRateMeasurementsQueryHandler(
    IMeasurementRepository<HeartRateMeasurementEntity> repository,
    ISessionService session
) : IRequestHandler<GetHeartRateMeasurementsQuery, ApiResponse<List<HeartRateMeasurementEntity>>>
{
    public Task<ApiResponse<List<HeartRateMeasurementEntity>>> Handle(
        GetHeartRateMeasurementsQuery request,
        CancellationToken cancellationToken
    )
    {
        var userId = session.GetUserClaims().UserId;

        return new MeasurementFactory<HeartRateMeasurementEntity>(repository, userId)
            .GetMeasurementsAsync(cancellationToken);
    }
}