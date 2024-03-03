using FluentValidation;
using MediatR;
using SafeMedConnect.Application.Factories;
using SafeMedConnect.Domain.Entities;
using SafeMedConnect.Domain.Interfaces.Repositories;
using SafeMedConnect.Domain.Interfaces.Services;
using SafeMedConnect.Domain.Responses;

namespace SafeMedConnect.Application.Commands.BloodSugar;

public sealed record DeleteBloodSugarCommand(string Id)
    : IRequest<ResponseWrapper<List<BloodSugarMeasurementEntity>>>;

// TODO: Change to simplified version
public class DeleteBloodSugarCommandHandler(
    IMeasurementRepository<BloodSugarEntity, BloodSugarMeasurementEntity> repository,
    ISessionService session
) : IRequestHandler<DeleteBloodSugarCommand, ResponseWrapper<List<BloodSugarMeasurementEntity>>>
{
    public Task<ResponseWrapper<List<BloodSugarMeasurementEntity>>> Handle(
        DeleteBloodSugarCommand request,
        CancellationToken cancellationToken
    ) => new MeasurementFactory<BloodSugarEntity, BloodSugarMeasurementEntity>(session, repository)
        .DeleteMeasurementAsync(request.Id, cancellationToken);
}

public sealed class DeleteBloodSugarCommandValidator : AbstractValidator<DeleteBloodSugarCommand>
{
    public DeleteBloodSugarCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}