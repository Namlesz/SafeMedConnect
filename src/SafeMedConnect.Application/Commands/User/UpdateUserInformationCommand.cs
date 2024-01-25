using MediatR;
using SafeMedConnect.Domain.Entities;
using SafeMedConnect.Domain.Responses;

namespace SafeMedConnect.Application.Commands.User;

public sealed class UpdateUserInformationCommand : IRequest<ResponseWrapper<UserEntity>>
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public double? Weight { get; set; }
    public double? Height { get; set; }
    public string? BloodType { get; set; }
    public List<string>? Allergies { get; set; }
    public List<string>? Medications { get; set; }
    public string? HealthInsuranceNumber { get; set; }
}

// TODO: Use session service to find user by id
// TODO: Update user information model
public class UpdateUserInformationCommandHandler : IRequestHandler<UpdateUserInformationCommand, ResponseWrapper<UserEntity>>
{
    public Task<ResponseWrapper<UserEntity>> Handle(UpdateUserInformationCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}