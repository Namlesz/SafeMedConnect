using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using SafeMedConnect.Domain.Attributes;

namespace SafeMedConnect.Domain.Entities;

[RepositoryName("AppUsers")]
public sealed class ApplicationUserEntity
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("password_hash")]
    public string PasswordHash { get; set; } = null!;

    [BsonElement("email")]
    public string Email { get; set; } = null!;

    [BsonElement("user_id")]
    public string UserId { get; set; } = null!;

    [BsonElement("mfa_enabled")]
    public bool MfaEnabled { get; set; } = false;

    [BsonElement("mfa_secret")]
    public string? MfaSecret { get; set; }
}