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
    public async Task<bool> RegisterUserAsync(ApplicationUserEntity appUser, CancellationToken cnl = default)
    {
        try
        {
            var userInfo = new UserEntity();
            await db.Users.InsertOneAsync(userInfo, cancellationToken: cnl);
            if (userInfo.Id is null)
            {
                throw new InvalidOperationException("UserInfo not created");
            }

            appUser.UserId = userInfo.Id;
            await db.ApplicationUsers.InsertOneAsync(appUser, cancellationToken: cnl);
            return true;
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Error while registering user");
            return false;
        }
    }

    public async Task<ApplicationUserEntity?> GetUserAsync(string email, CancellationToken cnl = default) =>
        await db.ApplicationUsers.AsQueryable()
            .FirstOrDefaultAsync(x => x.Email.Equals(email, StringComparison.CurrentCultureIgnoreCase), cancellationToken: cnl);
}