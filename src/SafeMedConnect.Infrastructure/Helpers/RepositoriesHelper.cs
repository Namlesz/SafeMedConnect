namespace SafeMedConnect.Infrastructure.Helpers;

internal static class RepositoryHelper
{
    public static string GetCollectionName<T>() where T : class =>
        typeof(T).Name.Replace("Entity", "s");
}