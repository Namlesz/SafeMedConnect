using AutoMapper;
using MediatR;
using SafeMedConnect.Application.Dto;
using SafeMedConnect.Domain.Abstract.Repositories;
using SafeMedConnect.Domain.Abstract.Services;
using SafeMedConnect.Domain.Enums;
using SafeMedConnect.Domain.Models;
using static SafeMedConnect.Domain.ClaimTypes.SharedDataClaimTypes;

namespace SafeMedConnect.Application.Queries.Share;

public record GetSharedUserInformationQuery : IRequest<ApiResponse<UserDto>>;

public class GetSharedUserInformationQueryHandler(
    ISessionService session,
    IMapper mapper,
    IUserRepository userRepository
) : IRequestHandler<GetSharedUserInformationQuery, ApiResponse<UserDto>>
{
    public async Task<ApiResponse<UserDto>> Handle(GetSharedUserInformationQuery request, CancellationToken cancellationToken)
    {
        var guestClaims = session.GetGuestClaims();
        if (guestClaims.DataShareClaims is null or { Count: 0 })
        {
            return new ApiResponse<UserDto>(ApiResponseTypes.InvalidRequest, "No data to share");
        }

        guestClaims.DataShareClaims.TryGetValue(ShareSensitiveData, out var shareBloodPressureMeasurement);
        if (!shareBloodPressureMeasurement)
        {
            return new ApiResponse<UserDto>(ApiResponseTypes.Forbidden);
        }

        var userInformation = await userRepository.GetUserAsync(guestClaims.UserId, cancellationToken);
        if (userInformation is null)
        {
            return new ApiResponse<UserDto>(ApiResponseTypes.NotFound);
        }

        var result = mapper.Map<UserDto>(userInformation);
        return new ApiResponse<UserDto>(ApiResponseTypes.Success, result);
    }
}