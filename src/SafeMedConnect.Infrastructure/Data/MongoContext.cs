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
        _database.GetCollection<ApplicationUserEntity>("AppUsers");

    public IMongoCollection<UserEntity> Users =>
        _database.GetCollection<UserEntity>(GetCollectionName<UserEntity>());

    public IMongoCollection<HeartRateEntity> HeartRates =>
        _database.GetCollection<HeartRateEntity>(GetCollectionName<HeartRateEntity>());

    public IMongoCollection<BloodPressureEntity> BloodPressures =>
        _database.GetCollection<BloodPressureEntity>(GetCollectionName<BloodPressureEntity>());

    public IMongoCollection<BloodPressureEntity> Temperatures =>
        _database.GetCollection<BloodPressureEntity>(GetCollectionName<TemperatureEntity>());

    public IMongoCollection<BloodSugarEntity> BloodSugars =>
        _database.GetCollection<BloodSugarEntity>(GetCollectionName<BloodSugarEntity>());

    public IMongoCollection<T> GetCollection<T>(string name) where T : class =>
        _database.GetCollection<T>(name);
}