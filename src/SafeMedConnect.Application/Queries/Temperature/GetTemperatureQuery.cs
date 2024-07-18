using MediatR;
using SafeMedConnect.Application.Factories;
using SafeMedConnect.Domain.Abstract.Repositories;
using SafeMedConnect.Domain.Abstract.Services;
using SafeMedConnect.Domain.Entities;
using SafeMedConnect.Domain.Models;

namespace SafeMedConnect.Application.Queries.Temperature;

public sealed record GetTemperatureQuery : IRequest<ApiResponse<List<TemperatureMeasurementEntity>>>;

public class GetTemperatureQueryHandler(
    ISessionService session,
    IMeasurementRepository<TemperatureMeasurementEntity> repository
) : IRequestHandler<GetTemperatureQuery, ApiResponse<List<TemperatureMeasurementEntity>>>
{
    public Task<ApiResponse<List<TemperatureMeasurementEntity>>> Handle(
        GetTemperatureQuery request,
        CancellationToken cancellationToken
    )
    {
        var userId = session.GetUserClaims().UserId;

        return new MeasurementFactory<TemperatureMeasurementEntity>(repository, userId)
            .GetMeasurementsAsync(cancellationToken);
    }
}