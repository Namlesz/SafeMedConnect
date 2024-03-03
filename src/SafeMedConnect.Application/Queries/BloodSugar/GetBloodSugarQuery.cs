using AutoMapper;
using MediatR;
using SafeMedConnect.Application.Dto;
using SafeMedConnect.Application.Factories;
using SafeMedConnect.Domain.Entities;
using SafeMedConnect.Domain.Interfaces.Repositories;
using SafeMedConnect.Domain.Interfaces.Services;
using SafeMedConnect.Domain.Responses;

namespace SafeMedConnect.Application.Queries.BloodSugar;

public sealed record GetBloodSugarQuery : IRequest<ResponseWrapper<List<BloodSugarMeasurementDto>>>;

public class GetBloodSugarQueryHandler(
    ISessionService session,
    IMeasurementRepository<BloodSugarMeasurementEntity> repository,
    IMapper mapper
) : IRequestHandler<GetBloodSugarQuery, ResponseWrapper<List<BloodSugarMeasurementDto>>>
{
    public Task<ResponseWrapper<List<BloodSugarMeasurementDto>>> Handle(
        GetBloodSugarQuery request,
        CancellationToken cancellationToken
    )
    {
        var userId = session.GetUserClaims().UserId;

        return new MeasurementFactoryWithMapper<BloodSugarMeasurementEntity>(repository, userId, mapper)
            .GetMeasurementsAsync<BloodSugarMeasurementDto>(cancellationToken);
    }
}