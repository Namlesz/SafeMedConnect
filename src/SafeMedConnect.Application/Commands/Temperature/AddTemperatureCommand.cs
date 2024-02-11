using AutoMapper;
using FluentValidation;
using MediatR;
using SafeMedConnect.Application.Factories;
using SafeMedConnect.Domain.Entities;
using SafeMedConnect.Domain.Interfaces.Repositories;
using SafeMedConnect.Domain.Interfaces.Services;
using SafeMedConnect.Domain.Responses;

namespace SafeMedConnect.Application.Commands.Temperature;

public sealed record AddTemperatureCommand(double Value, DateTime Timestamp)
    : IRequest<ResponseWrapper<List<TemperatureMeasurementEntity>>>;

public class AddTemperatureCommandHandler(
    IMeasurementRepository<TemperatureEntity, TemperatureMeasurementEntity> repository,
    ISessionService session,
    IMapper mapper
) : IRequestHandler<AddTemperatureCommand, ResponseWrapper<List<TemperatureMeasurementEntity>>>
{
    public async Task<ResponseWrapper<List<TemperatureMeasurementEntity>>> Handle(
        AddTemperatureCommand request,
        CancellationToken cancellationToken
    )
    {
        var factory = new MeasurementFactory<TemperatureEntity, TemperatureMeasurementEntity>(session, repository);
        var entity = mapper.Map<TemperatureMeasurementEntity>(request);
        return await factory.AddMeasurementAsync(entity, cancellationToken);
    }
}

public sealed class AddTemperatureCommandValidator : AbstractValidator<AddTemperatureCommand>
{
    public AddTemperatureCommandValidator()
    {
        RuleFor(x => x.Value).NotEmpty();
        RuleFor(x => x.Timestamp).NotEmpty();
    }
}