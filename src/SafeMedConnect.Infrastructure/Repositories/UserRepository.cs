using SafeMedConnect.Domain.Entities;
using SafeMedConnect.Domain.Interfaces.Repositories;
using SafeMedConnect.Infrastructure.Data;

namespace SafeMedConnect.Infrastructure.Repositories;

internal sealed class UserRepository(MongoContext db) : IUserRepository
{
    public async Task CreateUserAsync(string name)
    {
        var user = new UserEntity
        {
            Id = Guid.NewGuid(),
            Name = name,
            Email = "email@me.pl",
            PasswordHash = "hasz",
            PasswordSalt = "salt",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            DeletedAt = null
        };

        await db.Users.InsertOneAsync(user);
    }
}