namespace SafeMedConnect.Domain.Interfaces.Repositories;

public interface IUserRepository
{
    public Task CreateUserAsync(string name);
}