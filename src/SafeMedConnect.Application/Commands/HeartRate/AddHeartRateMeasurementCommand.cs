using AutoMapper;
using FluentValidation;
using MediatR;
using SafeMedConnect.Application.Factories;
using SafeMedConnect.Domain.Abstract.Repositories;
using SafeMedConnect.Domain.Abstract.Services;
using SafeMedConnect.Domain.Entities;
using SafeMedConnect.Domain.Models;

namespace SafeMedConnect.Application.Commands.HeartRate;

public sealed record AddHeartRateMeasurementCommand(DateTime Timestamp, int Value)
    : IRequest<ApiResponse<List<HeartRateMeasurementEntity>>>;

public class AddHeartRateMeasurementCommandHandler(
    IMeasurementRepository<HeartRateMeasurementEntity> repository,
    ISessionService session,
    IMapper mapper
) : IRequestHandler<AddHeartRateMeasurementCommand, ApiResponse<List<HeartRateMeasurementEntity>>>
{
    public async Task<ApiResponse<List<HeartRateMeasurementEntity>>> Handle(
        AddHeartRateMeasurementCommand request,
        CancellationToken cancellationToken
    )
    {
        var userId = session.GetUserClaims().UserId;
        var factory = new MeasurementFactory<HeartRateMeasurementEntity>(repository, userId);
        var measurementEntity = mapper.Map<HeartRateMeasurementEntity>(request);

        return await factory.AddMeasurementAsync(measurementEntity, cancellationToken);
    }
}

public class AddHeartRateMeasurementCommandValidator : AbstractValidator<AddHeartRateMeasurementCommand>
{
    public AddHeartRateMeasurementCommandValidator()
    {
        RuleFor(x => x.Timestamp).NotEmpty();
        RuleFor(x => x.Value).NotEmpty();
    }
}