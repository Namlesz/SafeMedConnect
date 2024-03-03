using MediatR;
using SafeMedConnect.Application.Factories;
using SafeMedConnect.Domain.Entities;
using SafeMedConnect.Domain.Interfaces.Repositories;
using SafeMedConnect.Domain.Interfaces.Services;
using SafeMedConnect.Domain.Responses;

namespace SafeMedConnect.Application.Queries.Temperature;

public sealed record GetTemperatureQuery : IRequest<ResponseWrapper<List<TemperatureMeasurementEntity>>>;

public class GetTemperatureQueryHandler(
    ISessionService session,
    IMeasurementRepository<TemperatureMeasurementEntity> repository
) : IRequestHandler<GetTemperatureQuery, ResponseWrapper<List<TemperatureMeasurementEntity>>>
{
    public Task<ResponseWrapper<List<TemperatureMeasurementEntity>>> Handle(
        GetTemperatureQuery request,
        CancellationToken cancellationToken
    )
    {
        var userId = session.GetUserClaims().UserId;

        return new MeasurementFactory<TemperatureMeasurementEntity>(repository, userId)
            .GetMeasurementsAsync(cancellationToken);
    }
}