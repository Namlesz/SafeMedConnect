using MediatR;

namespace SafeMedConnect.Application.Queries;

public sealed class GetAccountNameQuery : IRequest<string>
{
    public int AccountId { get; init; }
}

internal class GetAccountNameQueryHandler : IRequestHandler<GetAccountNameQuery, string>
{
    public Task<string> Handle(GetAccountNameQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult("Test");
    }
}