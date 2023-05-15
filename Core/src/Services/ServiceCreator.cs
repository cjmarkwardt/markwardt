namespace Markwardt;

public interface IServiceCreator
{
    ValueTask<object?> TryCreate(Type service, IEnumerable<Argument<object?>>? arguments = null, IEnumerable<Argument<Type>>? typeArguments = null);
}

public static class ServiceCreatorUtils
{
    public static async ValueTask<object> Create(this IServiceCreator creator, Type service, IEnumerable<Argument<object?>>? arguments = null, IEnumerable<Argument<Type>>? typeArguments = null)
        => (await creator.TryCreate(service, arguments, typeArguments)).NotNull($"Service {service} could not be created");

    public static async ValueTask<T?> TryCreate<T>(this IServiceCreator creator, IEnumerable<Argument<object?>>? arguments = null, IEnumerable<Argument<Type>>? typeArguments = null)
        where T : class
        => (T?) await creator.TryCreate(typeof(T), arguments, typeArguments);

    public static async ValueTask<T> Create<T>(this IServiceCreator creator, IEnumerable<Argument<object?>>? arguments = null, IEnumerable<Argument<Type>>? typeArguments = null)
        where T : class
        => (T) await creator.Create(typeof(T), arguments, typeArguments);
}