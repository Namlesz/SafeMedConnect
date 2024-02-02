using FluentValidation;
using MediatR;
using SafeMedConnect.Application.Factories;
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
    public Task<ResponseWrapper<List<HeartRateMeasurementEntity>>> Handle(
        DeleteHeartRateMeasurementCommand request,
        CancellationToken cancellationToken
    ) => new MeasurementFactory<HeartRateEntity, HeartRateMeasurementEntity>(session, repository)
        .DeleteMeasurementAsync(request.Id, cancellationToken);
}

public class DeleteHeartRateMeasurementCommandValidator : AbstractValidator<DeleteHeartRateMeasurementCommand>
{
    public DeleteHeartRateMeasurementCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}