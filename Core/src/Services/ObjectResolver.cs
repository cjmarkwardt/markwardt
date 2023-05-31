namespace Markwardt;

public interface IObjectResolver
{
    ValueTask<object?> TryResolve(Type target);
}

public static class ObjectResolverUtils
{
    public static async ValueTask<object> Resolve(this IObjectContainer container, Type target)
        => (await container.TryResolve(target)).NotNull($"Object of type {target} could not be resolved");

    public static async ValueTask<T?> TryResolve<T>(this IObjectContainer container)
        where T : class
        => (T?) await container.TryResolve(typeof(T));

    public static async ValueTask<T> Resolve<T>(this IObjectContainer container)
        where T : class
        => (T) await container.Resolve(typeof(T));
}