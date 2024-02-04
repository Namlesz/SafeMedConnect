using AutoMapper;
using MediatR;
using SafeMedConnect.Application.Dto;
using SafeMedConnect.Domain.Interfaces.Repositories;
using SafeMedConnect.Domain.Interfaces.Services;
using SafeMedConnect.Domain.Responses;

namespace SafeMedConnect.Application.Queries.User;

public record GetUserInformationQuery : IRequest<ResponseWrapper<UserDto>>;

public class GetUserInformationQueryHandler(IUserRepository repository, ISessionService session, IMapper mapper)
    : IRequestHandler<GetUserInformationQuery, ResponseWrapper<UserDto>>
{
    public async Task<ResponseWrapper<UserDto>> Handle(GetUserInformationQuery request, CancellationToken cancellationToken)
    {
        var userId = session.GetUserClaims().UserId;

        var entity = await repository.GetUserAsync(userId, cancellationToken);
        return entity is null
            ? new ResponseWrapper<UserDto>(ResponseTypes.NotFound)
            : new ResponseWrapper<UserDto>(ResponseTypes.Success, data: mapper.Map<UserDto>(entity));
    }
}