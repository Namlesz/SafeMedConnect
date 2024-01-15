using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using SafeMedConnect.Domain.Entities;

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

    public IMongoCollection<UserEntity> Users =>
        _database.GetCollection<UserEntity>("Users");
}