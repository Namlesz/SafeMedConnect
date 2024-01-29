using AutoMapper;
using FluentValidation;
using MediatR;
using SafeMedConnect.Domain.Entities;
using SafeMedConnect.Domain.Interfaces.Repositories;
using SafeMedConnect.Domain.Interfaces.Services;
using SafeMedConnect.Domain.Responses;

namespace SafeMedConnect.Application.Commands.HeartRate;

public sealed class AddHeartRateMeasurementCommand : IRequest<ResponseWrapper<List<HeartRateMeasurementEntity>>>
{
    public DateTime Timestamp { get; init; }
    public int Value { get; init; }
}

public class AddHeartRateMeasurementCommandHandler(
    IMeasurementRepository<HeartRateEntity, HeartRateMeasurementEntity> repository,
    ISessionService session,
    IMapper mapper
) : IRequestHandler<AddHeartRateMeasurementCommand, ResponseWrapper<List<HeartRateMeasurementEntity>>>
{
    public async Task<ResponseWrapper<List<HeartRateMeasurementEntity>>> Handle(
        AddHeartRateMeasurementCommand request, CancellationToken cancellationToken)
    {
        var userId = session.GetUserClaims().UserId;
        var heartRateMeasurement = mapper.Map<HeartRateMeasurementEntity>(request);

        var result = await repository.AddOrUpdateAsync(userId, heartRateMeasurement, cancellationToken);
        return result?.Measurements is null
            ? new ResponseWrapper<List<HeartRateMeasurementEntity>>(
                ResponseTypes.Error,
                "Error while adding heart rate measurement"
            )
            : new ResponseWrapper<List<HeartRateMeasurementEntity>>(
                ResponseTypes.Success,
                data: result.Measurements
            );
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