using AutoMapper;
using FluentValidation;
using MediatR;
using SafeMedConnect.Application.Factories;
using SafeMedConnect.Domain.Entities;
using SafeMedConnect.Domain.Enums;
using SafeMedConnect.Domain.Interfaces.Repositories;
using SafeMedConnect.Domain.Interfaces.Services;
using SafeMedConnect.Domain.Responses;

namespace SafeMedConnect.Application.Commands.BloodSugar;

public sealed record AddBloodSugarCommand(decimal Value, GlucoseUnitType GlucoseUnit, BloodSugarMeasurementMethodType MeasurementMethod, DateTime Timestamp)
    : IRequest<ResponseWrapper<List<BloodSugarMeasurementEntity>>>;

public class AddBloodSugarCommandHandler(
    IMeasurementRepository<BloodSugarEntity, BloodSugarMeasurementEntity> repository,
    ISessionService session,
    IMapper mapper
) : IRequestHandler<AddBloodSugarCommand, ResponseWrapper<List<BloodSugarMeasurementEntity>>>
{
    public async Task<ResponseWrapper<List<BloodSugarMeasurementEntity>>> Handle(
        AddBloodSugarCommand request,
        CancellationToken cancellationToken
    )
    {
        var factory = new MeasurementFactory<BloodSugarEntity, BloodSugarMeasurementEntity>(session, repository);
        var entity = mapper.Map<BloodSugarMeasurementEntity>(request);
        return await factory.AddMeasurementAsync(entity, cancellationToken);
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