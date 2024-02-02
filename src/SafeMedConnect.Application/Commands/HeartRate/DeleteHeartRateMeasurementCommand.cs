using FluentValidation;
using MediatR;
using SafeMedConnect.Domain.Entities;
using SafeMedConnect.Domain.Interfaces.Repositories;
using SafeMedConnect.Domain.Interfaces.Services;
using SafeMedConnect.Domain.Responses;

namespace SafeMedConnect.Application.Commands.HeartRate;

public sealed class DeleteHeartRateMeasurementCommand : IRequest<ResponseWrapper<List<HeartRateMeasurementEntity>>>
{
    public string Id { get; init; } = null!;
}

public class DeleteHeartRateMeasurementCommandHandler(
    IMeasurementRepository<HeartRateEntity, HeartRateMeasurementEntity> repository,
    ISessionService session
) : IRequestHandler<DeleteHeartRateMeasurementCommand, ResponseWrapper<List<HeartRateMeasurementEntity>>>
{
    public async Task<ResponseWrapper<List<HeartRateMeasurementEntity>>> Handle(
        DeleteHeartRateMeasurementCommand request,
        CancellationToken cancellationToken
    )
    {
        var userId = session.GetUserClaims().UserId;

        var entity = await repository.GetAsync(userId, cancellationToken);
        if (entity?.Measurements is null)
        {
            return new ResponseWrapper<List<HeartRateMeasurementEntity>>(ResponseTypes.Error);
        }

        var measurementToDelete = entity.Measurements.FirstOrDefault(x => x.Id == request.Id);
        if (measurementToDelete is null)
        {
            return new ResponseWrapper<List<HeartRateMeasurementEntity>>(ResponseTypes.NotFound);
        }

        entity.Measurements.Remove(measurementToDelete);

        var result = await repository.UpdateAsync(entity, cancellationToken);
        return result?.Measurements is null
            ? new ResponseWrapper<List<HeartRateMeasurementEntity>>(
                ResponseTypes.Error,
                "Error while deleting heart rate measurement"
            )
            : new ResponseWrapper<List<HeartRateMeasurementEntity>>(
                ResponseTypes.Success,
                data: result.Measurements
            );
    }
}

public class DeleteHeartRateMeasurementCommandValidator : AbstractValidator<DeleteHeartRateMeasurementCommand>
{
    public DeleteHeartRateMeasurementCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}