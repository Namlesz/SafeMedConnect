using FluentValidation;
using MediatR;
using SafeMedConnect.Application.Factories;
using SafeMedConnect.Domain.Entities;
using SafeMedConnect.Domain.Interfaces.Repositories;
using SafeMedConnect.Domain.Interfaces.Services;
using SafeMedConnect.Domain.Responses;

namespace SafeMedConnect.Application.Commands.Temperature;

public sealed record DeleteTemperatureCommand(string Id)
    : IRequest<ResponseWrapper<List<TemperatureMeasurementEntity>>>;

public class DeleteTemperatureCommandHandler(
    IMeasurementRepository<TemperatureMeasurementEntity> repository,
    ISessionService session
) : IRequestHandler<DeleteTemperatureCommand, ResponseWrapper<List<TemperatureMeasurementEntity>>>
{
    public Task<ResponseWrapper<List<TemperatureMeasurementEntity>>> Handle(
        DeleteTemperatureCommand request,
        CancellationToken cancellationToken
    )
    {
        var userId = session.GetUserClaims().UserId;

        return new MeasurementFactory<TemperatureMeasurementEntity>(repository, userId)
            .DeleteMeasurementAsync(request.Id, cancellationToken);
    }
}

public sealed class DeleteTemperatureCommandValidator : AbstractValidator<DeleteTemperatureCommand>
{
    public DeleteTemperatureCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}