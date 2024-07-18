using FluentValidation;
using MediatR;
using SafeMedConnect.Application.Factories;
using SafeMedConnect.Domain.Abstract.Repositories;
using SafeMedConnect.Domain.Abstract.Services;
using SafeMedConnect.Domain.Entities;
using SafeMedConnect.Domain.Models;

namespace SafeMedConnect.Application.Commands.HeartRate;

public sealed record DeleteHeartRateMeasurementCommand(string Id)
    : IRequest<ApiResponse<List<HeartRateMeasurementEntity>>>;

public class DeleteHeartRateMeasurementCommandHandler(
    IMeasurementRepository<HeartRateMeasurementEntity> repository,
    ISessionService session
) : IRequestHandler<DeleteHeartRateMeasurementCommand, ApiResponse<List<HeartRateMeasurementEntity>>>
{
    public Task<ApiResponse<List<HeartRateMeasurementEntity>>> Handle(
        DeleteHeartRateMeasurementCommand request,
        CancellationToken cancellationToken
    )
    {
        var userId = session.GetUserClaims().UserId;

        return new MeasurementFactory<HeartRateMeasurementEntity>(repository, userId)
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