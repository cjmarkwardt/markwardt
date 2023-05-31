namespace Markwardt;

public static class TypeUtils
{
    public static bool IsInstantiable(this Type type)
        => !type.IsInterface && !type.IsAbstract && !type.IsDelegate();

    public static Type? TryGetGenericTypeDefinition(this Type type)
        => type.IsGenericType ? type.GetGenericTypeDefinition() : null;

    public static Type[]? TryGetGenericArguments(this Type type)
        => type.IsGenericType ? type.GetGenericArguments() : null;

    public static bool TryGetType(string name, [NotNullWhen(true)] out Type? type)
    {
        type = Type.GetType(name);
        return type != null;
    }
}