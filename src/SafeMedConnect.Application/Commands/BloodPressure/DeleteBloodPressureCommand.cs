using FluentValidation;
using MediatR;
using SafeMedConnect.Application.Factories;
using SafeMedConnect.Domain.Entities;
using SafeMedConnect.Domain.Interfaces.Repositories;
using SafeMedConnect.Domain.Interfaces.Services;
using SafeMedConnect.Domain.Responses;

namespace SafeMedConnect.Application.Commands.BloodPressure;

public sealed record DeleteBloodPressureCommand(string Id)
    : IRequest<ResponseWrapper<List<BloodPressureMeasurementEntity>>>;

public class DeleteBloodPressureCommandHandler(
    IMeasurementRepositorySimplified<BloodPressureMeasurementEntity> repository,
    ISessionService session
) : IRequestHandler<DeleteBloodPressureCommand, ResponseWrapper<List<BloodPressureMeasurementEntity>>>
{
    public Task<ResponseWrapper<List<BloodPressureMeasurementEntity>>> Handle(
        DeleteBloodPressureCommand request,
        CancellationToken cancellationToken
    )
    {
        var userId = session.GetUserClaims().UserId;

        return new MeasurementFactorySimplified<BloodPressureMeasurementEntity>(repository, userId)
            .DeleteMeasurementAsync(request.Id, cancellationToken);
    }
}

public sealed class DeleteBloodPressureCommandValidator : AbstractValidator<DeleteBloodPressureCommand>
{
    public DeleteBloodPressureCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}