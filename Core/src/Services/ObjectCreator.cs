namespace Markwardt;

public interface IObjectCreator
{
    ValueTask<Maybe<object?>> Create(ObjectTag tag, Maybe<IObjectArgumentGenerator> arguments = default);
}

public static class ObjectCreatorUtils
{
    public static async ValueTask<Maybe<T>> Create<T, TScheme>(this IObjectCreator creator, Maybe<IObjectArgumentGenerator> arguments = default)
        where TScheme : IObjectScheme<T>
        => (await creator.Create(new ObjectTag(typeof(T), typeof(TScheme)), arguments)).Cast<T>();

    public static async ValueTask<Maybe<T>> Create<T>(this IObjectCreator creator, Maybe<IObjectArgumentGenerator> arguments = default)
        => (await creator.Create(typeof(T), arguments)).Cast<T>();
}