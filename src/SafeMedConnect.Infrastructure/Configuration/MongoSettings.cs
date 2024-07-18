using System.ComponentModel.DataAnnotations;

namespace SafeMedConnect.Infrastructure.Configuration;

public sealed class MongoSettings
{
    [Required]
    public string ConnectionString { get; init; } = null!;

    [Required]
    public string DefaultDatabase { get; init; } = null!;
}