namespace Markwardt;

public interface IServiceConfiguration : IServiceContainer
{
    void Configure(Type service, IServiceTarget target);
}

public static class ServiceConfigurationUtils
{
    public static void Configure(this IServiceConfiguration configuration, IServicePackage package)
        => package.Configure(configuration);

    public static void Configure(this IServiceConfiguration configuration, Type service, Type implementation, ServiceLifetime lifetime = ServiceLifetime.Singleton)
        => configuration.Configure(service, new ServiceTarget(lifetime, new ClassTargetBuilder(implementation)));

    public static void Configure(this IServiceConfiguration configuration, Type service, object instance)
        => configuration.Configure(service, new ServiceTarget(ServiceLifetime.Singleton, new InstanceTargetBuilder(instance)));

    public static void Configure<TService, TImplementation>(this IServiceConfiguration configuration, ServiceLifetime lifetime = ServiceLifetime.Singleton)
        where TService : class
        where TImplementation : class, TService
        => configuration.Configure(typeof(TService), typeof(TImplementation), lifetime);

    public static void Configure<TService>(this IServiceConfiguration configuration, TService instance)
        where TService : class
        => configuration.Configure(typeof(TService), instance);

    public static void ConfigureRoute(this IServiceConfiguration configuration, Type service, Type targetService, ServiceLifetime lifetime = ServiceLifetime.Singleton)
        => configuration.Configure(service, new ServiceTarget(lifetime, lifetime == ServiceLifetime.Singleton ? configuration.CreateResolveBuilder(targetService) : configuration.CreateBuilder(targetService)));

    public static void ConfigureRoute<TService, TTargetService>(this IServiceConfiguration configuration, ServiceLifetime lifetime = ServiceLifetime.Singleton)
        where TService : class
        where TTargetService : class, TService
        => configuration.ConfigureRoute(typeof(TService), typeof(TTargetService), lifetime);

    public static async ValueTask<object> BuildRoot(this IServiceConfiguration configuration, Type rootType)
        => await new ClassTargetBuilder(rootType).Build(configuration);

    public static async ValueTask<T> BuildRoot<T>(this IServiceConfiguration configuration)
        where T : class
        => (T) await configuration.BuildRoot(typeof(T));
}

public class ServiceConfiguration : ManagedAsyncDisposable, IServiceConfiguration
{
    private readonly Dictionary<Type, IServiceTarget> targets = new();
    private readonly Dictionary<Type, AsyncLazy<object>> singletons = new();

    public void Configure(Type service, IServiceTarget target)
        => targets[service] = target;

    public async ValueTask<object?> TryCreate(Type service, IEnumerable<Argument<object?>>? arguments = null, IEnumerable<Argument<Type>>? typeArguments = null)
        => TryGetTarget(service, out IServiceTarget? target) ? await target.Builder.Build(this, arguments, typeArguments) : null;

    public async ValueTask<object?> TryResolve(Type service)
    {
        if (service == typeof(IServiceCreator) || service == typeof(IServiceContainer) || service == typeof(IServiceConfiguration))
        {
            return this;
        }

        if (!TryGetTarget(service, out IServiceTarget? target))
        {
            return null;
        }

        IEnumerable<Argument<Type>>? typeArguments = null;
        if (service.IsGenericType)
        {
            typeArguments = service.GetGenericTypeDefinition().GetGenericArguments().Zip(service.GetGenericArguments()).Select(x => new Argument<Type>(x.First.Name, x.Second));
        }

        if (target.Lifetime == ServiceLifetime.Singleton)
        {
            if (!singletons.TryGetValue(service, out AsyncLazy<object>? singleton))
            {
                singleton = new AsyncLazy<object>(async () => await target.Builder.Build(this, null, typeArguments));
                Disposal.Track(singleton);
                singletons.Add(service, singleton);
            }

            return await singleton.GetValue();
        }
        else
        {
            return await target.Builder.Build(this, null, typeArguments);
        }
    }

    private bool TryGetTarget(Type service, [NotNullWhen(true)] out IServiceTarget? target)
    {
        if (targets.TryGetValue(service, out target))
        {
            return true;
        }

        if (service.IsGenericType && targets.TryGetValue(service.GetGenericTypeDefinition(), out target))
        {
            return true;
        }

        target = TryGenerateTarget(service);
        if (target != null)
        {
            targets.Add(service, target);
            return true;
        }

        return false;
    }

    protected virtual IServiceTarget? TryGenerateTarget(Type service)
    {
        if (service.TryGetGenericTypeDefinition() == typeof(Factory<>))
        {
            return new FactoryTarget(this, service.GetGenericArguments().First());
        }

        return ServiceTarget.FromServiceType(this, service);
    }

    protected override void OnDisposal()
    {
        base.OnDisposal();

        foreach (AsyncLazy<object> singleton in singletons.Values)
        {
            ValueTask<object> getValue = singleton.GetValue();
            if (getValue.IsCompleted)
            {
                getValue.Result.TryDispose();
            }
        }
    }

    protected override async ValueTask OnAsyncDisposal()
    {
        await base.OnAsyncDisposal();

        foreach (AsyncLazy<object> singleton in singletons.Values)
        {
            await (await singleton.GetValue()).TryDisposeAsync();
        }
    }
}