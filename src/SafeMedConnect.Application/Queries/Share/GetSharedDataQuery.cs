using MediatR;
using SafeMedConnect.Domain.Responses;
using System.Diagnostics;

namespace SafeMedConnect.Application.Queries.Share;

public sealed record GetSharedDataQuery : IRequest<ResponseWrapper<object>>;

public class GetSharedDataQueryHandler : IRequestHandler<GetSharedDataQuery, ResponseWrapper<object>>
{
    public Task<ResponseWrapper<object>> Handle(GetSharedDataQuery request, CancellationToken cancellationToken)
    {
        Debug.WriteLine("GetSharedDataQueryHandler");
        throw new NotImplementedException();
    }
}