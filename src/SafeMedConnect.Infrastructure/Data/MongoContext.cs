using Microsoft.Extensions.Options;
using MongoDB.Driver;
using SafeMedConnect.Domain.Configuration;
using SafeMedConnect.Domain.Entities;
using static SafeMedConnect.Infrastructure.Helpers.RepositoryHelper;

namespace SafeMedConnect.Infrastructure.Data;

internal sealed class MongoContext
{
    private readonly IMongoDatabase _database;

    public MongoContext(IOptions<MongoSettings> mongoSettingsOptions)
    {
        var mongoSettings = mongoSettingsOptions.Value;

        _database = new MongoClient(mongoSettings.ConnectionString)
            .GetDatabase(mongoSettings.DefaultDatabase);
    }

    public IMongoCollection<ApplicationUserEntity> ApplicationUsers =>
        _database.GetCollection<ApplicationUserEntity>(GetCollectionName<ApplicationUserEntity>());

    public IMongoCollection<UserEntity> Users =>
        _database.GetCollection<UserEntity>(GetCollectionName<UserEntity>());

    public IMongoCollection<T> GetCollection<T>(string name) where T : class =>
        _database.GetCollection<T>(name);
}