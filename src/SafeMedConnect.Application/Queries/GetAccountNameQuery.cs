using MediatR;
using SafeMedConnect.Domain.Interfaces.Repositories;

namespace SafeMedConnect.Application.Queries;

public sealed class GetAccountNameQuery : IRequest<string>
{
    public int AccountId { get; init; }
}

internal class GetAccountNameQueryHandler(IUserRepository repo) : IRequestHandler<GetAccountNameQuery, string>
{
    public Task<string> Handle(GetAccountNameQuery request, CancellationToken cancellationToken)
    {
        try
        {
            repo.CreateUserAsync(request.AccountId.ToString());
            return Task.FromResult("OK");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}