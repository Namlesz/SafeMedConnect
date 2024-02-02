using MediatR;
using SafeMedConnect.Domain.Entities;
using SafeMedConnect.Domain.Interfaces.Repositories;
using SafeMedConnect.Domain.Interfaces.Services;
using SafeMedConnect.Domain.Responses;

namespace SafeMedConnect.Application.Queries.HeartRate;

public record GetHeartRateMeasurementsQuery : IRequest<ResponseWrapper<List<HeartRateMeasurementEntity>>>;

public class GetHeartRateMeasurementsQueryHandler(
    IMeasurementRepository<HeartRateEntity, HeartRateMeasurementEntity> repository,
    ISessionService session
) : IRequestHandler<GetHeartRateMeasurementsQuery, ResponseWrapper<List<HeartRateMeasurementEntity>>>
{
    public async Task<ResponseWrapper<List<HeartRateMeasurementEntity>>> Handle(
        GetHeartRateMeasurementsQuery request,
        CancellationToken cancellationToken
    )
    {
        var userId = session.GetUserClaims().UserId;
        var entity = await repository.GetAsync(userId, cnl: cancellationToken);

        return entity?.Measurements is null
            ? new ResponseWrapper<List<HeartRateMeasurementEntity>>(ResponseTypes.NotFound)
            : new ResponseWrapper<List<HeartRateMeasurementEntity>>(ResponseTypes.Success, data: entity.Measurements);
    }
}