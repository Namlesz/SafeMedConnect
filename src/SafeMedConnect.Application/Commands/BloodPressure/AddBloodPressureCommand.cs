using AutoMapper;
using FluentValidation;
using MediatR;
using SafeMedConnect.Application.Factories;
using SafeMedConnect.Domain.Entities;
using SafeMedConnect.Domain.Interfaces.Repositories;
using SafeMedConnect.Domain.Interfaces.Services;
using SafeMedConnect.Domain.Responses;

namespace SafeMedConnect.Application.Commands.BloodPressure;

public sealed class AddBloodPressureCommand : IRequest<ResponseWrapper<List<BloodPressureMeasurementEntity>>>
{
    public int Systolic { get; init; }
    public int Diastolic { get; init; }
    public int HeartRate { get; init; }
    public DateTime Timestamp { get; init; }
}

public class AddBloodPressureCommandHandler(
    IMeasurementRepository<BloodPressureEntity, BloodPressureMeasurementEntity> repository,
    ISessionService session,
    IMapper mapper
) : IRequestHandler<AddBloodPressureCommand, ResponseWrapper<List<BloodPressureMeasurementEntity>>>
{
    public async Task<ResponseWrapper<List<BloodPressureMeasurementEntity>>> Handle(
        AddBloodPressureCommand request,
        CancellationToken cancellationToken
    )
    {
        var factory = new MeasurementFactory<BloodPressureEntity, BloodPressureMeasurementEntity>(session, repository);
        var entity = mapper.Map<BloodPressureMeasurementEntity>(request);
        return await factory.AddMeasurementAsync(entity, cancellationToken);
    }
}

public sealed class AddBloodPressureCommandValidator : AbstractValidator<AddBloodPressureCommand>
{
    public AddBloodPressureCommandValidator()
    {
        RuleFor(x => x.Systolic).NotEmpty();
        RuleFor(x => x.Diastolic).NotEmpty();
        RuleFor(x => x.HeartRate).NotEmpty();
        RuleFor(x => x.Timestamp).NotEmpty();
    }
}