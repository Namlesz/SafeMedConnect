namespace SafeMedConnect.Domain.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public sealed class RepositoryNameAttribute(string name) : Attribute
{
    public string Name { get; } = name;
}