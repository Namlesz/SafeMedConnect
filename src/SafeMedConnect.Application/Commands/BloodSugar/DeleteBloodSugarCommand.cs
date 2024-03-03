using AutoMapper;
using FluentValidation;
using MediatR;
using SafeMedConnect.Application.Dto;
using SafeMedConnect.Application.Factories;
using SafeMedConnect.Domain.Entities;
using SafeMedConnect.Domain.Interfaces.Repositories;
using SafeMedConnect.Domain.Interfaces.Services;
using SafeMedConnect.Domain.Responses;

namespace SafeMedConnect.Application.Commands.BloodSugar;

public sealed record DeleteBloodSugarCommand(string Id)
    : IRequest<ResponseWrapper<List<BloodSugarMeasurementDto>>>;

public class DeleteBloodSugarCommandHandler(
    IMeasurementRepository<BloodSugarMeasurementEntity> repository,
    ISessionService session,
    IMapper mapper
) : IRequestHandler<DeleteBloodSugarCommand, ResponseWrapper<List<BloodSugarMeasurementDto>>>
{
    public Task<ResponseWrapper<List<BloodSugarMeasurementDto>>> Handle(
        DeleteBloodSugarCommand request,
        CancellationToken cancellationToken
    )
    {
        var userId = session.GetUserClaims().UserId;

        return new MeasurementFactoryWithMapper<BloodSugarMeasurementEntity>(repository, userId, mapper)
            .DeleteMeasurementAsync<BloodSugarMeasurementDto>(request.Id, cancellationToken);
    }
}

public sealed class DeleteBloodSugarCommandValidator : AbstractValidator<DeleteBloodSugarCommand>
{
    public DeleteBloodSugarCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}