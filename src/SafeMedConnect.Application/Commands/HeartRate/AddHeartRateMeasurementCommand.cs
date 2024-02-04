using AutoMapper;
using FluentValidation;
using MediatR;
using SafeMedConnect.Application.Factories;
using SafeMedConnect.Domain.Entities;
using SafeMedConnect.Domain.Interfaces.Repositories;
using SafeMedConnect.Domain.Interfaces.Services;
using SafeMedConnect.Domain.Responses;

namespace SafeMedConnect.Application.Commands.HeartRate;

public sealed record AddHeartRateMeasurementCommand(DateTime Timestamp, int Value)
    : IRequest<ResponseWrapper<List<HeartRateMeasurementEntity>>>;

public class AddHeartRateMeasurementCommandHandler(
    IMeasurementRepository<HeartRateEntity, HeartRateMeasurementEntity> repository,
    ISessionService session,
    IMapper mapper
) : IRequestHandler<AddHeartRateMeasurementCommand, ResponseWrapper<List<HeartRateMeasurementEntity>>>
{
    public async Task<ResponseWrapper<List<HeartRateMeasurementEntity>>> Handle(
        AddHeartRateMeasurementCommand request, CancellationToken cancellationToken)
    {
        var factory = new MeasurementFactory<HeartRateEntity, HeartRateMeasurementEntity>(session, repository);
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