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

    public static Type? TryGetType(string name)
        => TryGetType(name, out Type? type) ? type : null;

    public static Type GetType(string name)
        => TryGetType(name) ?? throw new InvalidOperationException();

    public static bool TryGetInterfaceImplementation(this Type interfaceType, [NotNullWhen(true)] out Type? implementationType)
        => TryGetType(interfaceType.AssemblyQualifiedName.Replace(interfaceType.Name, interfaceType.Name.Substring(1)), out implementationType);

    public static Type? TryGetInterfaceImplementation(this Type interfaceType)
        => TryGetInterfaceImplementation(interfaceType, out Type? type) ? type : null;

    public static Type GetInterfaceImplementation(this Type interfaceType)
        => TryGetInterfaceImplementation(interfaceType) ?? throw new InvalidOperationException();

    public static bool TryCreate<T>(this Type? type, [NotNullWhen(true)] out T? obj)
        where T : notnull
    {
        if (type == null)
        {
            obj = default;
            return false;
        }

        try
        {
            obj = (T)Activator.CreateInstance(type);
            return true;
        }
        catch
        {
            obj = default;
            return false;
        }
    }

    public static T? TryCreate<T>(this Type? type)
        where T : notnull
        => TryCreate(type, out T? obj) ? obj : default;

    public static T Create<T>(this Type? type)
        where T : notnull
        => TryCreate<T>(type) ?? throw new InvalidOperationException();
}