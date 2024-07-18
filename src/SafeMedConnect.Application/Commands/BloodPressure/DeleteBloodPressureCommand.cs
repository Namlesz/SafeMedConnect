using FluentValidation;
using MediatR;
using SafeMedConnect.Application.Factories;
using SafeMedConnect.Domain.Abstract.Repositories;
using SafeMedConnect.Domain.Abstract.Services;
using SafeMedConnect.Domain.Entities;
using SafeMedConnect.Domain.Models;

namespace SafeMedConnect.Application.Commands.BloodPressure;

public sealed record DeleteBloodPressureCommand(string Id)
    : IRequest<ApiResponse<List<BloodPressureMeasurementEntity>>>;

public class DeleteBloodPressureCommandHandler(
    IMeasurementRepository<BloodPressureMeasurementEntity> repository,
    ISessionService session
) : IRequestHandler<DeleteBloodPressureCommand, ApiResponse<List<BloodPressureMeasurementEntity>>>
{
    public Task<ApiResponse<List<BloodPressureMeasurementEntity>>> Handle(
        DeleteBloodPressureCommand request,
        CancellationToken cancellationToken
    )
    {
        var userId = session.GetUserClaims().UserId;

        return new MeasurementFactory<BloodPressureMeasurementEntity>(repository, userId)
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