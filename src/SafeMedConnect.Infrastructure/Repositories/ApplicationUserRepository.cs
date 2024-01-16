using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using SafeMedConnect.Domain.Entities;
using SafeMedConnect.Domain.Interfaces.Repositories;
using SafeMedConnect.Infrastructure.Data;

namespace SafeMedConnect.Infrastructure.Repositories;

internal sealed class ApplicationUserRepository(MongoContext db, ILogger<ApplicationUserRepository> logger)
    : IApplicationUserRepository
{
    public async Task<bool> RegisterUserAsync(ApplicationUserEntity user, CancellationToken cnl = default)
    {
        try
        {
            await db.ApplicationUsers.InsertOneAsync(user, cancellationToken: cnl);
            return true;
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Error while registering user");
            return false;
        }
    }

    public async Task<ApplicationUserEntity?> GetUserAsync(string login, CancellationToken cnl = default) =>
        await db.ApplicationUsers.AsQueryable()
            .FirstOrDefaultAsync(x => x.Login == login, cancellationToken: cnl);
}