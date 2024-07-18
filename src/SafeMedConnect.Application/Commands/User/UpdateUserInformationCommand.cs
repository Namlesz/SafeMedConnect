using AutoMapper;
using MediatR;
using SafeMedConnect.Application.Dto;
using SafeMedConnect.Domain.Abstract.Repositories;
using SafeMedConnect.Domain.Abstract.Services;
using SafeMedConnect.Domain.Entities;
using SafeMedConnect.Domain.Enums;
using SafeMedConnect.Domain.Models;

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
) : IRequest<ApiResponse<UserDto>>;

public class UpdateUserInformationCommandHandler(IUserRepository repository, ISessionService session, IMapper mapper)
    : IRequestHandler<UpdateUserInformationCommand, ApiResponse<UserDto>>
{
    public async Task<ApiResponse<UserDto>> Handle(
        UpdateUserInformationCommand request,
        CancellationToken cancellationToken
    )
    {
        var user = mapper.Map<UserEntity>(request);
        var userId = session.GetUserClaims().UserId;

        user.Id = userId;

        var updatedUser = await repository.UpdateUserAsync(user, cancellationToken);
        return updatedUser is null
            ? new ApiResponse<UserDto>(ApiResponseTypes.Error, "Error while updating user")
            : new ApiResponse<UserDto>(ApiResponseTypes.Success, mapper.Map<UserDto>(updatedUser));
    }
}