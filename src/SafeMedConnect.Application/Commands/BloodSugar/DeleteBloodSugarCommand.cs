using AutoMapper;
using FluentValidation;
using MediatR;
using SafeMedConnect.Application.Dto.Measurements;
using SafeMedConnect.Application.Factories;
using SafeMedConnect.Domain.Abstract.Repositories;
using SafeMedConnect.Domain.Abstract.Services;
using SafeMedConnect.Domain.Entities;
using SafeMedConnect.Domain.Models;

namespace SafeMedConnect.Application.Commands.BloodSugar;

public sealed record DeleteBloodSugarCommand(string Id)
    : IRequest<ApiResponse<List<BloodSugarMeasurementDto>>>;

public class DeleteBloodSugarCommandHandler(
    IMeasurementRepository<BloodSugarMeasurementEntity> repository,
    ISessionService session,
    IMapper mapper
) : IRequestHandler<DeleteBloodSugarCommand, ApiResponse<List<BloodSugarMeasurementDto>>>
{
    public Task<ApiResponse<List<BloodSugarMeasurementDto>>> Handle(
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