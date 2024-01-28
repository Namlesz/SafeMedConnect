using MediatR;
using SafeMedConnect.Domain.Entities;
using SafeMedConnect.Domain.Interfaces.Repositories;
using SafeMedConnect.Domain.Interfaces.Services;
using SafeMedConnect.Domain.Responses;

namespace SafeMedConnect.Application.Queries.HeartRate;

public record GetHeartRateMeasurementsQuery : IRequest<ResponseWrapper<List<HeartRateMeasurementEntity>>>;

public class GetHeartRateMeasurementsQueryHandler(IHeartRatesRepository repository, ISessionService session)
    : IRequestHandler<GetHeartRateMeasurementsQuery, ResponseWrapper<List<HeartRateMeasurementEntity>>>
{
    public async Task<ResponseWrapper<List<HeartRateMeasurementEntity>>> Handle(
        GetHeartRateMeasurementsQuery request,
        CancellationToken cancellationToken
    )
    {
        var userId = session.GetUserClaims().UserId;
        var result = await repository.GetHeartRateMeasurementsAsync(userId, cnl: cancellationToken);

        return result?.Measurements is null
            ? new ResponseWrapper<List<HeartRateMeasurementEntity>>(ResponseTypes.NotFound)
            : new ResponseWrapper<List<HeartRateMeasurementEntity>>(ResponseTypes.Success, data: result.Measurements);
    }
}