using AutoMapper;
using MediatR;
using SafeMedConnect.Application.Dto;
using SafeMedConnect.Domain.Entities;
using SafeMedConnect.Domain.Interfaces.Repositories;
using SafeMedConnect.Domain.Interfaces.Services;
using SafeMedConnect.Domain.Responses;

namespace SafeMedConnect.Application.Commands.User;

public sealed record UpdateUserInformationCommand(
    string? FirstName,
    string? LastName,
    DateTime? DateOfBirth,
    double? Weight,
    double? Height,
    string? BloodType,
    List<string>? Allergies,
    List<string>? Medications,
    string? HealthInsuranceNumber,
    List<string>? DiagnosedDiseases
) : IRequest<ResponseWrapper<UserDto>>;

public class UpdateUserInformationCommandHandler(IUserRepository repository, ISessionService session, IMapper mapper)
    : IRequestHandler<UpdateUserInformationCommand, ResponseWrapper<UserDto>>
{
    public async Task<ResponseWrapper<UserDto>> Handle(
        UpdateUserInformationCommand request,
        CancellationToken cancellationToken
    )
    {
        var user = mapper.Map<UserEntity>(request);
        var userId = session.GetUserClaims().UserId;

        user.Id = userId;

        var updatedUser = await repository.UpdateUserAsync(user, cancellationToken);
        return updatedUser is null
            ? new ResponseWrapper<UserDto>(ResponseTypes.Error, "Error while updating user")
            : new ResponseWrapper<UserDto>(ResponseTypes.Success, data: mapper.Map<UserDto>(updatedUser));
    }
}