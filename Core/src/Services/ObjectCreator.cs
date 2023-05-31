namespace Markwardt;

public interface IObjectCreator
{
    ValueTask<object?> TryCreate(Type target, IArgumentGenerator? arguments = null);
}

public static class ObjectCreatorUtils
{
    public static async ValueTask<object> Create(this IObjectCreator creator, Type target, IArgumentGenerator? arguments = null)
        => (await creator.TryCreate(target, arguments)).NotNull($"Service {target} could not be created");

    public static async ValueTask<T?> TryCreate<T>(this IObjectCreator creator, IArgumentGenerator? arguments = null)
        where T : class
        => (T?) await creator.TryCreate(typeof(T), arguments);

    public static async ValueTask<T> Create<T>(this IObjectCreator creator, IArgumentGenerator? arguments = null)
        where T : class
        => (T) await creator.Create(typeof(T), arguments);
}