using FluentValidation;
using MediatR;
using SafeMedConnect.Application.Factories;
using SafeMedConnect.Domain.Entities;
using SafeMedConnect.Domain.Interfaces.Repositories;
using SafeMedConnect.Domain.Interfaces.Services;
using SafeMedConnect.Domain.Responses;

namespace SafeMedConnect.Application.Commands.BloodPressure;

public sealed class DeleteBloodPressureCommand : IRequest<ResponseWrapper<List<BloodPressureMeasurementEntity>>>
{
    public string Id { get; init; } = null!;
}

public class DeleteBloodPressureCommandHandler(
    IMeasurementRepository<BloodPressureEntity, BloodPressureMeasurementEntity> repository,
    ISessionService session
) : IRequestHandler<DeleteBloodPressureCommand, ResponseWrapper<List<BloodPressureMeasurementEntity>>>
{
    public Task<ResponseWrapper<List<BloodPressureMeasurementEntity>>> Handle(
        DeleteBloodPressureCommand request,
        CancellationToken cancellationToken
    ) => new MeasurementFactory<BloodPressureEntity, BloodPressureMeasurementEntity>(session, repository)
        .DeleteMeasurementAsync(request.Id, cancellationToken);
}

public sealed class DeleteBloodPressureCommandValidator : AbstractValidator<DeleteBloodPressureCommand>
{
    public DeleteBloodPressureCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
