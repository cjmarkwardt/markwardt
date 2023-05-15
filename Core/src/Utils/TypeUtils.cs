namespace Markwardt;

public static class TypeUtils
{
    public static Type? TryGetGenericTypeDefinition(this Type type)
        => type.IsGenericType ? type.GetGenericTypeDefinition() : null;

    public static Type[]? TryGetGenericArguments(this Type type)
        => type.IsGenericType ? type.GetGenericArguments() : null;
}