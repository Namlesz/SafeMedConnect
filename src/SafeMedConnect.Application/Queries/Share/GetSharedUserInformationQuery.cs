using AutoMapper;
using MediatR;
using SafeMedConnect.Application.Dto;
using SafeMedConnect.Domain.Interfaces.Repositories;
using SafeMedConnect.Domain.Interfaces.Services;
using SafeMedConnect.Domain.Responses;
using static SafeMedConnect.Domain.ClaimTypes.DataShareClaimTypes;

namespace SafeMedConnect.Application.Queries.Share;

public record GetSharedUserInformationQuery : IRequest<ResponseWrapper<UserDto>>;

public class GetSharedUserInformationQueryHandler(
    ISessionService session,
    IMapper mapper,
    IUserRepository userRepository
) : IRequestHandler<GetSharedUserInformationQuery, ResponseWrapper<UserDto>>
{
    public async Task<ResponseWrapper<UserDto>> Handle(GetSharedUserInformationQuery request, CancellationToken cancellationToken)
    {
        var guestClaims = session.GetGuestClaims();
        if (guestClaims.DataShareClaims is null or { Count: 0 })
        {
            return new ResponseWrapper<UserDto>(ResponseTypes.InvalidRequest, message: "No data to share");
        }

        guestClaims.DataShareClaims.TryGetValue(ShareBloodPressureMeasurement, out var shareBloodPressureMeasurement);
        if (!shareBloodPressureMeasurement)
        {
            return new ResponseWrapper<UserDto>(ResponseTypes.Forbidden);
        }

        var userInformation = await userRepository.GetUserAsync(guestClaims.UserId, cancellationToken);
        if (userInformation is null)
        {
            return new ResponseWrapper<UserDto>(ResponseTypes.NotFound);
        }

        var result = mapper.Map<UserDto>(userInformation);
        return new ResponseWrapper<UserDto>(ResponseTypes.Success, result);
    }
}