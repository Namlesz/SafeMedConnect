using AutoMapper;
using MediatR;
using SafeMedConnect.Domain.Entities;
using SafeMedConnect.Domain.Interfaces.Repositories;
using SafeMedConnect.Domain.Interfaces.Services;
using SafeMedConnect.Domain.Responses;

namespace SafeMedConnect.Application.Commands.User;

public sealed class UpdateUserInformationCommand : IRequest<ResponseWrapper<UserEntity>>
{
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
    public DateTime? DateOfBirth { get; init; }
    public double? Weight { get; init; }
    public double? Height { get; init; }
    public string? BloodType { get; init; }
    public List<string>? Allergies { get; init; }
    public List<string>? Medications { get; init; }
    public string? HealthInsuranceNumber { get; init; }
}

public class UpdateUserInformationCommandHandler(IUserRepository repository, ISessionService session, IMapper mapper)
    : IRequestHandler<UpdateUserInformationCommand, ResponseWrapper<UserEntity>>
{
    public async Task<ResponseWrapper<UserEntity>> Handle(
        UpdateUserInformationCommand request,
        CancellationToken cancellationToken
    )
    {
        var user = mapper.Map<UserEntity>(request);
        var userId = session.GetUserClaims().UserId;

        user.Id = userId;

        var updatedUser = await repository.UpdateUserAsync(user, cancellationToken);
        return updatedUser is null
            ? new ResponseWrapper<UserEntity>(ResponseTypes.Error, "Error while updating user")
            : new ResponseWrapper<UserEntity>(ResponseTypes.Success, data: updatedUser);
    }
}