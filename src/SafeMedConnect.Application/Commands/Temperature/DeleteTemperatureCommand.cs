using FluentValidation;
using MediatR;
using SafeMedConnect.Application.Factories;
using SafeMedConnect.Domain.Abstract.Repositories;
using SafeMedConnect.Domain.Abstract.Services;
using SafeMedConnect.Domain.Entities;
using SafeMedConnect.Domain.Models;

namespace SafeMedConnect.Application.Commands.Temperature;

public sealed record DeleteTemperatureCommand(string Id)
    : IRequest<ApiResponse<List<TemperatureMeasurementEntity>>>;

public class DeleteTemperatureCommandHandler(
    IMeasurementRepository<TemperatureMeasurementEntity> repository,
    ISessionService session
) : IRequestHandler<DeleteTemperatureCommand, ApiResponse<List<TemperatureMeasurementEntity>>>
{
    public Task<ApiResponse<List<TemperatureMeasurementEntity>>> Handle(
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