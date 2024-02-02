using MediatR;
using SafeMedConnect.Application.Factories;
using SafeMedConnect.Domain.Entities;
using SafeMedConnect.Domain.Interfaces.Repositories;
using SafeMedConnect.Domain.Interfaces.Services;
using SafeMedConnect.Domain.Responses;

namespace SafeMedConnect.Application.Queries.BloodPressure;

public sealed class GetBloodPressureQuery : IRequest<ResponseWrapper<List<BloodPressureMeasurementEntity>>>;

public class GetBloodPressureQueryHandler(
    ISessionService session,
    IMeasurementRepository<BloodPressureEntity, BloodPressureMeasurementEntity> repository
) : IRequestHandler<GetBloodPressureQuery, ResponseWrapper<List<BloodPressureMeasurementEntity>>>
{
    public Task<ResponseWrapper<List<BloodPressureMeasurementEntity>>> Handle(
        GetBloodPressureQuery request, CancellationToken cancellationToken
    ) => new MeasurementFactory<BloodPressureEntity, BloodPressureMeasurementEntity>(session, repository)
        .GetMeasurementsAsync(cancellationToken);
}