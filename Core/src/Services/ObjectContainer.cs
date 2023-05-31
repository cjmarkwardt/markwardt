namespace Markwardt;

public interface IObjectContainer : IObjectCreator, IObjectResolver, IMultiDisposable
{
    ValueTask<object?> TryGet(Type target);
}

public static class ObjectContainerUtils
{
    public static async ValueTask<object> Get(this IObjectContainer container, Type target)
        => (await container.TryGet(target)).NotNull($"Object of type {target} could not be retrieved");

    public static async ValueTask<T?> TryGet<T>(this IObjectContainer container)
        where T : class
        => (T?) await container.TryGet(typeof(T));

    public static async ValueTask<T> Get<T>(this IObjectContainer container)
        where T : class
        => (T) await container.Get(typeof(T));
}