using AutoMapper;
using MediatR;
using SafeMedConnect.Application.Dto.Measurements;
using SafeMedConnect.Application.Factories;
using SafeMedConnect.Domain.Abstract.Repositories;
using SafeMedConnect.Domain.Abstract.Services;
using SafeMedConnect.Domain.Entities;
using SafeMedConnect.Domain.Models;

namespace SafeMedConnect.Application.Queries.BloodSugar;

public sealed record GetBloodSugarQuery : IRequest<ApiResponse<List<BloodSugarMeasurementDto>>>;

public class GetBloodSugarQueryHandler(
    ISessionService session,
    IMeasurementRepository<BloodSugarMeasurementEntity> repository,
    IMapper mapper
) : IRequestHandler<GetBloodSugarQuery, ApiResponse<List<BloodSugarMeasurementDto>>>
{
    public Task<ApiResponse<List<BloodSugarMeasurementDto>>> Handle(
        GetBloodSugarQuery request,
        CancellationToken cancellationToken
    )
    {
        var userId = session.GetUserClaims().UserId;

        return new MeasurementFactoryWithMapper<BloodSugarMeasurementEntity>(repository, userId, mapper)
            .GetMeasurementsAsync<BloodSugarMeasurementDto>(cancellationToken);
    }
}