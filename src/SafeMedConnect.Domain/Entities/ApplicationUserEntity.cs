using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SafeMedConnect.Domain.Entities;

public sealed class ApplicationUserEntity
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("Login")]
    public string Login { get; set; } = null!;

    [BsonElement("PasswordHash")]
    public string PasswordHash { get; set; } = null!;
}