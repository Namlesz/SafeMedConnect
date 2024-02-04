namespace SafeMedConnect.Api.Attributes;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
internal sealed class ApiRouteAttribute(string route) : Attribute
{
    public string Route { get; } = route;
}