using MediatR;
using SafeMedConnect.Domain.Entities;
using SafeMedConnect.Domain.Interfaces.Repositories;
using SafeMedConnect.Domain.Interfaces.Services;
using SafeMedConnect.Domain.Responses;

namespace SafeMedConnect.Application.Queries.User;

public record GetUserInformationQuery : IRequest<ResponseWrapper<UserEntity>>;

public class GetUserInformationQueryHandler(IUserRepository repository, ISessionService session)
    : IRequestHandler<GetUserInformationQuery, ResponseWrapper<UserEntity>>
{
    public async Task<ResponseWrapper<UserEntity>> Handle(GetUserInformationQuery request, CancellationToken cancellationToken)
    {
        var userId = session.GetUserClaims().UserId;
        if (userId is null)
        {
            return new ResponseWrapper<UserEntity>(ResponseTypes.Error, "Missing user id in token claims");
        }

        var result = await repository.GetUserAsync(userId, cancellationToken);
        return result is null
            ? new ResponseWrapper<UserEntity>(ResponseTypes.NotFound)
            : new ResponseWrapper<UserEntity>(ResponseTypes.Success, data: result);
    }
}