using MediatR;
using SafeMedConnect.Application.Factories;
using SafeMedConnect.Domain.Entities;
using SafeMedConnect.Domain.Interfaces.Repositories;
using SafeMedConnect.Domain.Interfaces.Services;
using SafeMedConnect.Domain.Responses;

namespace SafeMedConnect.Application.Queries.HeartRate;

public record GetHeartRateMeasurementsQuery : IRequest<ResponseWrapper<List<HeartRateMeasurementEntity>>>;

public class GetHeartRateMeasurementsQueryHandler(
    IMeasurementRepository<HeartRateMeasurementEntity> repository,
    ISessionService session
) : IRequestHandler<GetHeartRateMeasurementsQuery, ResponseWrapper<List<HeartRateMeasurementEntity>>>
{
    public Task<ResponseWrapper<List<HeartRateMeasurementEntity>>> Handle(
        GetHeartRateMeasurementsQuery request,
        CancellationToken cancellationToken
    )
    {
        var userId = session.GetUserClaims().UserId;

        return new MeasurementFactory<HeartRateMeasurementEntity>(repository, userId)
            .GetMeasurementsAsync(cancellationToken);
    }
}