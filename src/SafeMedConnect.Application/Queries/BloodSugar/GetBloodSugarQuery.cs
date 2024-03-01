using AutoMapper;
using MediatR;
using SafeMedConnect.Domain.Entities;
using SafeMedConnect.Domain.Interfaces.Repositories;
using SafeMedConnect.Domain.Interfaces.Services;
using SafeMedConnect.Domain.Responses;

namespace SafeMedConnect.Application.Queries.BloodSugar;

public sealed record GetBloodSugarQuery : IRequest<ResponseWrapper<List<BloodSugarMeasurementDto>>>;

public class GetBloodSugarQueryHandler(
    ISessionService session,
    IMeasurementRepository<BloodSugarEntity, BloodSugarMeasurementEntity> repository,
    IMapper mapper
) : IRequestHandler<GetBloodSugarQuery, ResponseWrapper<List<BloodSugarMeasurementDto>>>
{
    public async Task<ResponseWrapper<List<BloodSugarMeasurementDto>>> Handle(
        GetBloodSugarQuery request,
        CancellationToken cancellationToken
    )
    {
        var entity = await repository.GetAsync(session.GetUserClaims().UserId, cancellationToken);

        if (entity?.Measurements is null)
        {
            return new ResponseWrapper<List<BloodSugarMeasurementDto>>(ResponseTypes.NotFound);
        }

        return new ResponseWrapper<List<BloodSugarMeasurementDto>>(
            ResponseTypes.Success,
            mapper.Map<List<BloodSugarMeasurementDto>>(entity.Measurements)
        );
    }
}

public sealed record BloodSugarMeasurementDto(
    string Id,
    decimal Value,
    DateTime Timestamp,
    string Unit,
    string MeasurementMethod
);