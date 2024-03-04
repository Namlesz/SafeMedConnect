using AutoMapper;
using FluentValidation;
using MediatR;
using SafeMedConnect.Application.Dto;
using SafeMedConnect.Application.Factories;
using SafeMedConnect.Domain.Entities;
using SafeMedConnect.Domain.Enums;
using SafeMedConnect.Domain.Interfaces.Repositories;
using SafeMedConnect.Domain.Interfaces.Services;
using SafeMedConnect.Domain.Responses;

namespace SafeMedConnect.Application.Commands.BloodSugar;

public sealed record AddBloodSugarCommand(
    decimal Value,
    GlucoseUnitType GlucoseUnit,
    BloodSugarMeasurementMethodType MeasurementMethod,
    DateTime Timestamp
) : IRequest<ResponseWrapper<List<BloodSugarMeasurementDto>>>;

public class AddBloodSugarCommandHandler(
    IMeasurementRepository<BloodSugarMeasurementEntity> repository,
    ISessionService session,
    IMapper mapper
) : IRequestHandler<AddBloodSugarCommand, ResponseWrapper<List<BloodSugarMeasurementDto>>>
{
    public async Task<ResponseWrapper<List<BloodSugarMeasurementDto>>> Handle(
        AddBloodSugarCommand request,
        CancellationToken cancellationToken
    )
    {
        var userId = session.GetUserClaims().UserId;

        var factory = new MeasurementFactoryWithMapper<BloodSugarMeasurementEntity>(repository, userId, mapper);
        var entity = mapper.Map<BloodSugarMeasurementEntity>(request);

        return await factory.AddMeasurementAsync<BloodSugarMeasurementDto>(entity, cancellationToken);
    }
}

public sealed class AddBloodSugarCommandValidator : AbstractValidator<AddBloodSugarCommand>
{
    public AddBloodSugarCommandValidator()
    {
        RuleFor(x => x.GlucoseUnit).IsInEnum();
        RuleFor(x => x.MeasurementMethod).IsInEnum();
        RuleFor(x => x.Value).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Timestamp).NotEmpty();
    }
}