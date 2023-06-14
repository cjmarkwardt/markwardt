namespace Markwardt;

public interface IObjectResolver : IObjectCreator
{
    ValueTask<Maybe<object?>> Resolve(ObjectTag tag);
}

public static class ObjectResolverUtils
{
    public static async ValueTask<Maybe<T>> Resolve<T, TScheme>(this IObjectResolver container)
        where TScheme : IObjectScheme<T>
        => (await container.Resolve(new ObjectTag(typeof(T), typeof(TScheme)))).Cast<T>();

    public static async ValueTask<Maybe<T>> Resolve<T>(this IObjectResolver container)
        => (await container.Resolve(typeof(T))).Cast<T>();
}