using FluentValidation;
using MediatR;
using SafeMedConnect.Application.Factories;
using SafeMedConnect.Domain.Entities;
using SafeMedConnect.Domain.Interfaces.Repositories;
using SafeMedConnect.Domain.Interfaces.Services;
using SafeMedConnect.Domain.Responses;

namespace SafeMedConnect.Application.Commands.HeartRate;

public sealed record DeleteHeartRateMeasurementCommand(string Id)
    : IRequest<ResponseWrapper<List<HeartRateMeasurementEntity>>>;

public class DeleteHeartRateMeasurementCommandHandler(
    IMeasurementRepositorySimplified<HeartRateMeasurementEntity> repository,
    ISessionService session
) : IRequestHandler<DeleteHeartRateMeasurementCommand, ResponseWrapper<List<HeartRateMeasurementEntity>>>
{
    public Task<ResponseWrapper<List<HeartRateMeasurementEntity>>> Handle(
        DeleteHeartRateMeasurementCommand request,
        CancellationToken cancellationToken
    )
    {
        var userId = session.GetUserClaims().UserId;

        return new MeasurementFactorySimplified<HeartRateMeasurementEntity>(repository, userId)
            .DeleteMeasurementAsync(request.Id, cancellationToken);
    }
}

public class DeleteHeartRateMeasurementCommandValidator : AbstractValidator<DeleteHeartRateMeasurementCommand>
{
    public DeleteHeartRateMeasurementCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}