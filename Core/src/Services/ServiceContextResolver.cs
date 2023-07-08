namespace Markwardt;

public interface IServiceContextResolver
{
    ValueTask<object?> TryResolve(ServiceTag tag, Action<IServiceContainer> configure);
}

public static class ServiceContextResolverUtils
{
    public static async ValueTask<object> Resolve(this IServiceContextResolver resolver, ServiceTag tag, Action<IServiceContainer> configure)
        => await resolver.TryResolve(tag, configure) ?? throw new InvalidOperationException();

    public static async ValueTask<IMaybe<T>> TryResolve<T>(this IServiceContextResolver resolver, Action<IServiceContainer> configure)
        where T : notnull
        => (await resolver.TryResolve(ServiceTag.Create<T>(), configure)).AsNullableMaybe().Cast<T>();

    public static async ValueTask<T> Resolve<T>(this IServiceContextResolver resolver, Action<IServiceContainer> configure)
        where T : notnull
        => (await resolver.TryResolve<T>(configure)).Value;

    public static async ValueTask<IMaybe<T>> TryResolve<T, TConfiguration>(this IServiceContextResolver resolver, Action<IServiceContainer> configure)
        where T : notnull
        where TConfiguration : IServiceConfiguration, new()
        => (await resolver.TryResolve(ServiceTag.Create<T, TConfiguration>(), configure)).AsNullableMaybe().Cast<T>();

    public static async ValueTask<T> Resolve<T, TConfiguration>(this IServiceContextResolver resolver, Action<IServiceContainer> configure)
        where T : notnull
        where TConfiguration : IServiceConfiguration, new()
        => (await resolver.TryResolve<T, TConfiguration>(configure)).Value;
}

public class ServiceContextResolver : IServiceContextResolver
{
    public ServiceContextResolver(IServiceContainer parent)
    {
        this.parent = parent;
    }

    private readonly IServiceContainer parent;

    public async ValueTask<object?> TryResolve(ServiceTag tag, Action<IServiceContainer> configure)
        => await parent.TryResolve(tag, configure);
}