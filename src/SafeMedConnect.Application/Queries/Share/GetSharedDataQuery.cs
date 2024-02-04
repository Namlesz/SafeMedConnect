using MediatR;
using SafeMedConnect.Domain.Responses;

namespace SafeMedConnect.Application.Queries.Share;

public sealed class GetSharedDataQuery : IRequest<ResponseWrapper<object>>
{
}

public class GetSharedDataQueryHandler : IRequestHandler<GetSharedDataQuery, ResponseWrapper<object>>
{
    public Task<ResponseWrapper<object>> Handle(GetSharedDataQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}