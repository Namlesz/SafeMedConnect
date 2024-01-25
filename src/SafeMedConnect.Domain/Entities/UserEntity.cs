using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SafeMedConnect.Domain.Entities;

public sealed class UserEntity
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("first_name")]
    public string? FirstName { get; set; }

    [BsonElement("last_name")]
    public string? LastName { get; set; }

    [BsonElement("date_of_birth")]
    public DateTime? DateOfBirth { get; set; }

    [BsonElement("weight")]
    public double? Weight { get; set; }

    [BsonElement("height")]
    public double? Height { get; set; }

    // TODO: Change to enum?
    [BsonElement("blood_type")]
    public string? BloodType { get; set; }

    [BsonElement("allergies")]
    public List<string>? Allergies { get; set; }

    [BsonElement("medications")]
    public List<string>? Medications { get; set; }

    [BsonElement("health_insurance_number")]
    public string? HealthInsuranceNumber { get; set; }
}