using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using SafeMedConnect.Domain.Entities;
using static SafeMedConnect.Common.Utilities.RepositoryHelper;

namespace SafeMedConnect.Infrastructure.Data;

internal sealed class MongoContext
{
    private readonly IMongoDatabase _database;

    public MongoContext(IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("HadesDb")
            ?? throw new InvalidOperationException("Missing connection string for HadesDb");

        var databaseName = configuration["DefaultDatabase"]
            ?? throw new InvalidOperationException("Missing database name");

        var client = new MongoClient(connectionString);

        _database = client.GetDatabase(databaseName);
    }

    public IMongoCollection<ApplicationUserEntity> ApplicationUsers =>
        _database.GetCollection<ApplicationUserEntity>("AppUsers");

    public IMongoCollection<UserEntity> Users =>
        _database.GetCollection<UserEntity>(GetCollectionName<UserEntity>());

    public IMongoCollection<HeartRateEntity> HeartRates =>
        _database.GetCollection<HeartRateEntity>(GetCollectionName<HeartRateEntity>());

    public IMongoCollection<BloodPressureEntity> BloodPressures =>
        _database.GetCollection<BloodPressureEntity>(GetCollectionName<BloodPressureEntity>());

    public IMongoCollection<T> GetCollection<T>(string name) where T : class =>
        _database.GetCollection<T>(name);
}