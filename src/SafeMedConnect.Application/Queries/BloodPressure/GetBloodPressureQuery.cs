using MediatR;
using SafeMedConnect.Application.Factories;
using SafeMedConnect.Domain.Entities;
using SafeMedConnect.Domain.Interfaces.Repositories;
using SafeMedConnect.Domain.Interfaces.Services;
using SafeMedConnect.Domain.Responses;

namespace SafeMedConnect.Application.Queries.BloodPressure;

public sealed record GetBloodPressureQuery : IRequest<ResponseWrapper<List<BloodPressureMeasurementEntity>>>;

public class GetBloodPressureQueryHandler(
    ISessionService session,
    IMeasurementRepositorySimplified<BloodPressureMeasurementEntity> repository
) : IRequestHandler<GetBloodPressureQuery, ResponseWrapper<List<BloodPressureMeasurementEntity>>>
{
    public Task<ResponseWrapper<List<BloodPressureMeasurementEntity>>> Handle(
        GetBloodPressureQuery request,
        CancellationToken cancellationToken
    )
    {
        var userId = session.GetUserClaims().UserId;

        return new MeasurementFactorySimplified<BloodPressureMeasurementEntity>(repository, userId)
            .GetMeasurementsAsync(cancellationToken);
    }
}