using AutoMapper;
using MediatR;
using SafeMedConnect.Application.Dto;
using SafeMedConnect.Domain.Abstract.Repositories;
using SafeMedConnect.Domain.Abstract.Services;
using SafeMedConnect.Domain.Enums;
using SafeMedConnect.Domain.Models;

namespace SafeMedConnect.Application.Queries.User;

public record GetUserInformationQuery : IRequest<ApiResponse<UserDto>>;

public class GetUserInformationQueryHandler(IUserRepository repository, ISessionService session, IMapper mapper)
    : IRequestHandler<GetUserInformationQuery, ApiResponse<UserDto>>
{
    public async Task<ApiResponse<UserDto>> Handle(GetUserInformationQuery request, CancellationToken cancellationToken)
    {
        var userId = session.GetUserClaims().UserId;

        var entity = await repository.GetUserAsync(userId, cancellationToken);
        return entity is null
            ? new ApiResponse<UserDto>(ApiResponseTypes.NotFound)
            : new ApiResponse<UserDto>(ApiResponseTypes.Success, mapper.Map<UserDto>(entity));
    }
}