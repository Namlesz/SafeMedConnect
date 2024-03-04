using AutoMapper;
using FluentValidation;
using MediatR;
using SafeMedConnect.Application.Factories;
using SafeMedConnect.Domain.Entities;
using SafeMedConnect.Domain.Interfaces.Repositories;
using SafeMedConnect.Domain.Interfaces.Services;
using SafeMedConnect.Domain.Responses;

namespace SafeMedConnect.Application.Commands.BloodPressure;

public sealed record AddBloodPressureCommand(int Systolic, int Diastolic, int HeartRate, DateTime Timestamp)
    : IRequest<ResponseWrapper<List<BloodPressureMeasurementEntity>>>;

public class AddBloodPressureCommandHandler(
    IMeasurementRepository<BloodPressureMeasurementEntity> repository,
    ISessionService session,
    IMapper mapper
) : IRequestHandler<AddBloodPressureCommand, ResponseWrapper<List<BloodPressureMeasurementEntity>>>
{
    public async Task<ResponseWrapper<List<BloodPressureMeasurementEntity>>> Handle(
        AddBloodPressureCommand request,
        CancellationToken cancellationToken
    )
    {
        var userId = session.GetUserClaims().UserId;

        var factory = new MeasurementFactory<BloodPressureMeasurementEntity>(repository, userId);
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