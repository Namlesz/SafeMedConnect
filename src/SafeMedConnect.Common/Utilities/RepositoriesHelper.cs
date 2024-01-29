namespace SafeMedConnect.Common.Utilities;

public static class RepositoryHelper
{
    public static string GetCollectionName<T>() where T : class =>
        typeof(T).Name.Replace("Entity", "s");
}