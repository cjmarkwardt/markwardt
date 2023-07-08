namespace Markwardt;

[RoutedService<IServiceContainer>]
public interface IServiceResolver
{
    ValueTask<object?> TryResolve(ServiceTag tag);
}

public static class ServiceResolverUtils
{
    public static IServiceContainer DeriveContainer(this IServiceResolver resolver)
        => new ServiceContainer(new ResolverConfigurationGenerator(resolver));

    public static async ValueTask<object> Resolve(this IServiceResolver resolver, ServiceTag tag)
        => await resolver.TryResolve(tag) ?? throw new InvalidOperationException();

    public static async ValueTask<IMaybe<T>> TryResolve<T>(this IServiceResolver resolver)
        where T : notnull
        => (await resolver.TryResolve(ServiceTag.Create<T>())).AsNullableMaybe().Cast<T>();

    public static async ValueTask<T> Resolve<T>(this IServiceResolver resolver)
        where T : notnull
        => (await resolver.TryResolve<T>()).Value;

    public static async ValueTask<IMaybe<T>> TryResolve<T, TConfiguration>(this IServiceResolver resolver)
        where T : notnull
        where TConfiguration : IServiceConfiguration, new()
        => (await resolver.TryResolve(ServiceTag.Create<T, TConfiguration>())).AsNullableMaybe().Cast<T>();

    public static async ValueTask<T> Resolve<T, TConfiguration>(this IServiceResolver resolver)
        where T : notnull
        where TConfiguration : IServiceConfiguration, new()
        => (await resolver.TryResolve<T, TConfiguration>()).Value;
}