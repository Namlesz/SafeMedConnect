using AutoMapper;
using FluentValidation;
using MediatR;
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
        var userId = session.GetUserClaims().UserId;
        var bloodPressureMeasurement = mapper.Map<BloodPressureMeasurementEntity>(request);

        var result = await repository.AddOrUpdateAsync(userId, bloodPressureMeasurement, cnl: cancellationToken);
        return result?.Measurements is null
            ? new ResponseWrapper<List<BloodPressureMeasurementEntity>>(
                ResponseTypes.Error,
                "Error while adding blood pressure measurement"
            )
            : new ResponseWrapper<List<BloodPressureMeasurementEntity>>(
                ResponseTypes.Success,
                data: result.Measurements
            );
    }
}

public sealed class AddBloodPressureCommandValidator : AbstractValidator<AddBloodPressureCommand>
{
    public AddBloodPressureCommandValidator()
    {
        RuleFor(x => x.Systolic).NotEmpty();
        RuleFor(x => x.Diastolic).NotEmpty();
        RuleFor(x => x.HeartRate).NotEmpty();
    }
}