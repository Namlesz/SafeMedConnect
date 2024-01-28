using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SafeMedConnect.Domain.Entities;

public sealed class UserEntity
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("first_name")]
    public string? FirstName { get; init; }

    [BsonElement("last_name")]
    public string? LastName { get; init; }

    [BsonElement("date_of_birth")]
    public DateTime? DateOfBirth { get; init; }

    [BsonElement("weight")]
    public double? Weight { get; init; }

    [BsonElement("height")]
    public double? Height { get; init; }

    // TODO: Change to enum?
    [BsonElement("blood_type")]
    public string? BloodType { get; init; }

    [BsonElement("allergies")]
    public List<string>? Allergies { get; init; }

    [BsonElement("medications")]
    public List<string>? Medications { get; init; }

    [BsonElement("health_insurance_number")]
    public string? HealthInsuranceNumber { get; init; }

    [BsonElement("diagnosed_diseases")]
    public List<string>? DiagnosedDiseases { get; init; }
}