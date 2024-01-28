namespace SafeMedConnect.Api.Attributes;

[AttributeUsage(AttributeTargets.Class)]
internal sealed class ApiRouteAttribute(string route) : Attribute
{
    public string Route { get; } = route;
}