using SafeMedConnect.Domain.Attributes;
using System.Reflection;

namespace SafeMedConnect.Infrastructure.Helpers;

internal static class RepositoryHelper
{
    public static string GetCollectionName<T>()
    {
        var attribute = typeof(T).GetCustomAttribute<RepositoryNameAttribute>(true);
        if (attribute != null)
        {
            return attribute.Name;
        }

        throw new InvalidOperationException($"RepositoryNameAttribute not found for {typeof(T).Name}");
    }
}