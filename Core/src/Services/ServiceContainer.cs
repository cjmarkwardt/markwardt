namespace Markwardt;

public interface IServiceContainer : IServiceCreator, IMultiDisposable
{
    ValueTask<object?> TryResolve(Type service);
}

public static class ServiceContainerUtils
{
    public static async ValueTask<object> Resolve(this IServiceContainer container, Type service)
        => (await container.TryResolve(service)).NotNull($"Service {service} could not be resolved");

    public static async ValueTask<T?> TryResolve<T>(this IServiceContainer container)
        where T : class
        => (T?) await container.TryResolve(typeof(T));

    public static async ValueTask<T> Resolve<T>(this IServiceContainer container)
        where T : class
        => (T) await container.Resolve(typeof(T));
}