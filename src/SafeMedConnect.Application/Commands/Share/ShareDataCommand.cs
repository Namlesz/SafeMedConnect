using MediatR;
using SafeMedConnect.Domain.Responses;

namespace SafeMedConnect.Application.Commands.Share;

public sealed record ShareDataCommand()
    : IRequest<ResponseWrapper<object>>;

public class ShareDataCommandHandler : IRequestHandler<ShareDataCommand, ResponseWrapper<object>>
{
    public Task<ResponseWrapper<object>> Handle(ShareDataCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}