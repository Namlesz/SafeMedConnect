using AutoMapper;
using FluentValidation;
using MediatR;
using SafeMedConnect.Application.Factories;
using SafeMedConnect.Domain.Abstract.Repositories;
using SafeMedConnect.Domain.Abstract.Services;
using SafeMedConnect.Domain.Entities;
using SafeMedConnect.Domain.Models;

namespace SafeMedConnect.Application.Commands.Temperature;

public sealed record AddTemperatureCommand(double Value, DateTime Timestamp)
    : IRequest<ApiResponse<List<TemperatureMeasurementEntity>>>;

public class AddTemperatureCommandHandler(
    IMeasurementRepository<TemperatureMeasurementEntity> repository,
    ISessionService session,
    IMapper mapper
) : IRequestHandler<AddTemperatureCommand, ApiResponse<List<TemperatureMeasurementEntity>>>
{
    public async Task<ApiResponse<List<TemperatureMeasurementEntity>>> Handle(
        AddTemperatureCommand request,
        CancellationToken cancellationToken
    )
    {
        var userId = session.GetUserClaims().UserId;
        var factory = new MeasurementFactory<TemperatureMeasurementEntity>(repository, userId);
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