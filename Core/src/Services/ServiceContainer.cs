namespace Markwardt;

public interface IServiceContainer : IServiceResolver, IMultiDisposable
{
    void Configure(ServiceTag tag, IServiceConfiguration configuration);
    void Clear(ServiceTag tag);

    void Configure(IServicePackage? package)
    {
        if (package != null)
        {
            package.Configure(this);
        }
    }

    void Configure<T>(IServiceConfiguration configuration)
        where T : notnull
        => Configure(ServiceTag.Create<T>(), configuration);

    void ConfigureRoute<T, TTarget>(IServiceResolver? resolver = null)
        where T : notnull
        where TTarget : notnull
        => Configure<T>(new RouteConfiguration(ServiceTag.Create<TTarget>(), resolver));

    void ConfigureRoute<T, TTarget, TTargetConfiguration>(IServiceResolver? resolver = null)
        where T : notnull
        where TTarget : notnull
        where TTargetConfiguration : IServiceConfiguration, new()
        => Configure<T>(new RouteConfiguration(ServiceTag.Create<TTarget, TTargetConfiguration>(), resolver));

    void ConfigureInstance<T>(T instance, bool dispose = true)
        where T : notnull
        => Configure<T>(new InstanceConfiguration(instance, dispose));

    void ConfigureConstructor<T, TImplementation>(ServiceKind kind = ServiceKind.Singleton, IDictionary<string, object?>? arguments = null)
        where T : notnull
        where TImplementation : T
        => Configure<T>(new ConstructorConfiguration(typeof(TImplementation), kind, arguments));

    void ConfigureFactory<T>(AsyncFunc<IServiceResolver, IDictionary<string, object?>?, T> factory, ServiceKind kind = ServiceKind.Singleton)
        where T : notnull
        => Configure<T>(new FactoryConfiguration(async (resolver, arguments) => await factory(resolver, arguments), kind));

    void ConfigureFactory<T>(AsyncFunc<IServiceResolver, T> factory, ServiceKind kind = ServiceKind.Singleton)
        where T : notnull
        => ConfigureFactory<T>(async (resolver, _) => await factory(resolver), kind);

    void ConfigureFactory<T>(AsyncFunc<T> factory, ServiceKind kind = ServiceKind.Singleton)
        where T : notnull
        => ConfigureFactory<T>(async (_, _) => await factory(), kind);

    void ConfigureFactory<T>(Func<IServiceResolver, IDictionary<string, object?>?, T> factory, ServiceKind kind = ServiceKind.Singleton)
        where T : notnull
        => ConfigureFactory<T>((resolver, arguments) => new ValueTask<T>(factory(resolver, arguments)), kind);

    void ConfigureFactory<T>(Func<IServiceResolver, T> factory, ServiceKind kind = ServiceKind.Singleton)
        where T : notnull
        => ConfigureFactory<T>((resolver, _) => new ValueTask<T>(factory(resolver)), kind);

    void ConfigureFactory<T>(Func<T> factory, ServiceKind kind = ServiceKind.Singleton)
        where T : notnull
        => ConfigureFactory<T>((_, _) => new ValueTask<T>(factory()), kind);

    void ConfigureSource<T, TSource>(AsyncFunc<TSource, T> getter, ServiceKind kind = ServiceKind.Singleton)
        where T : notnull
        where TSource : notnull
        => Configure<T>(new SourceConfiguration(ServiceTag.Create<TSource>(), async source => await getter((TSource)source), kind));

    void ConfigureSource<T, TSource>(Func<TSource, T> getter, ServiceKind kind = ServiceKind.Singleton)
        where T : notnull
        where TSource : notnull
        => ConfigureSource<T, TSource>(source => new ValueTask<T>(getter(source)), kind);

    void ConfigureSource<T, TSource, TSourceConfiguration>(AsyncFunc<TSource, T> getter, ServiceKind kind = ServiceKind.Singleton)
        where T : notnull
        where TSource : notnull
        where TSourceConfiguration : IServiceConfiguration, new()
        => Configure<T>(new SourceConfiguration(ServiceTag.Create<TSource, TSourceConfiguration>(), async source => await getter((TSource)source), kind));

    void ConfigureSource<T, TSource, TSourceConfiguration>(Func<TSource, T> getter, ServiceKind kind = ServiceKind.Singleton)
        where T : notnull
        where TSource : notnull
        where TSourceConfiguration : IServiceConfiguration, new()
        => ConfigureSource<T, TSource, TSourceConfiguration>(source => new ValueTask<T>(getter(source)), kind);

    void ConfigureSpecific<T, TConfiguration>(IServiceConfiguration configuration)
        where T : notnull
        where TConfiguration : IServiceConfiguration, new()
        => Configure(ServiceTag.Create<T, TConfiguration>(), configuration);

    void ConfigureSpecificRoute<T, TConfiguration, TTarget>(IServiceResolver? resolver = null)
        where T : notnull
        where TConfiguration : IServiceConfiguration, new()
        where TTarget : notnull
        => ConfigureSpecific<T, TConfiguration>(new RouteConfiguration(ServiceTag.Create<TTarget>(), resolver));

    void ConfigureSpecificRoute<T, TConfiguration, TTarget, TTargetConfiguration>(IServiceResolver? resolver = null)
        where T : notnull
        where TConfiguration : IServiceConfiguration, new()
        where TTarget : notnull
        where TTargetConfiguration : IServiceConfiguration, new()
        => ConfigureSpecific<T, TConfiguration>(new RouteConfiguration(ServiceTag.Create<TTarget, TTargetConfiguration>(), resolver));

    void ConfigureSpecificInstance<T, TConfiguration>(T instance, bool dispose = true)
        where T : notnull
        where TConfiguration : IServiceConfiguration, new()
        => ConfigureSpecific<T, TConfiguration>(new InstanceConfiguration(instance, dispose));

    void ConfigureSpecificConstructor<T, TConfiguration, TImplementation>(ServiceKind kind = ServiceKind.Singleton, IDictionary<string, object?>? arguments = null)
        where T : notnull
        where TConfiguration : IServiceConfiguration, new()
        where TImplementation : T
        => ConfigureSpecific<T, TConfiguration>(new ConstructorConfiguration(typeof(TImplementation), kind, arguments));

    void ConfigureSpecificFactory<T, TConfiguration>(AsyncFunc<IServiceResolver, IDictionary<string, object?>?, T> factory, ServiceKind kind = ServiceKind.Singleton)
        where T : notnull
        where TConfiguration : IServiceConfiguration, new()
        => ConfigureSpecific<T, TConfiguration>(new FactoryConfiguration(async (resolver, arguments) => await factory(resolver, arguments), kind));

    void ConfigureSpecificFactory<T, TConfiguration>(AsyncFunc<IServiceResolver, T> factory, ServiceKind kind = ServiceKind.Singleton)
        where T : notnull
        where TConfiguration : IServiceConfiguration, new()
        => ConfigureSpecificFactory<T, TConfiguration>(async (resolver, _) => await factory(resolver), kind);

    void ConfigureSpecificFactory<T, TConfiguration>(AsyncFunc<T> factory, ServiceKind kind = ServiceKind.Singleton)
        where T : notnull
        where TConfiguration : IServiceConfiguration, new()
        => ConfigureSpecificFactory<T, TConfiguration>(async (_, _) => await factory(), kind);

    void ConfigureSpecificFactory<T, TConfiguration>(Func<IServiceResolver, IDictionary<string, object?>?, T> factory, ServiceKind kind = ServiceKind.Singleton)
        where T : notnull
        where TConfiguration : IServiceConfiguration, new()
        => ConfigureSpecificFactory<T, TConfiguration>((resolver, arguments) => new ValueTask<T>(factory(resolver, arguments)), kind);

    void ConfigureSpecificFactory<T, TConfiguration>(Func<IServiceResolver, T> factory, ServiceKind kind = ServiceKind.Singleton)
        where T : notnull
        where TConfiguration : IServiceConfiguration, new()
        => ConfigureSpecificFactory<T, TConfiguration>((resolver, _) => new ValueTask<T>(factory(resolver)), kind);

    void ConfigureSpecificFactory<T, TConfiguration>(Func<T> factory, ServiceKind kind = ServiceKind.Singleton)
        where T : notnull
        where TConfiguration : IServiceConfiguration, new()
        => ConfigureSpecificFactory<T, TConfiguration>((_, _) => new ValueTask<T>(factory()), kind);

    void ConfigureSpecificSource<T, TConfiguration, TSource>(AsyncFunc<TSource, T> getter, ServiceKind kind = ServiceKind.Singleton)
        where T : notnull
        where TConfiguration : IServiceConfiguration, new()
        where TSource : notnull
        => ConfigureSpecific<T, TConfiguration>(new SourceConfiguration(ServiceTag.Create<TSource>(), async source => await getter((TSource)source), kind));

    void ConfigureSpecificSource<T, TConfiguration, TSource>(Func<TSource, T> getter, ServiceKind kind = ServiceKind.Singleton)
        where T : notnull
        where TConfiguration : IServiceConfiguration, new()
        where TSource : notnull
        => ConfigureSpecificSource<T, TConfiguration, TSource>(source => new ValueTask<T>(getter(source)), kind);

    void ConfigureSpecificSource<T, TConfiguration, TSource, TSourceConfiguration>(AsyncFunc<TSource, T> getter, ServiceKind kind = ServiceKind.Singleton)
        where T : notnull
        where TConfiguration : IServiceConfiguration, new()
        where TSource : notnull
        where TSourceConfiguration : IServiceConfiguration, new()
        => ConfigureSpecific<T, TConfiguration>(new SourceConfiguration(ServiceTag.Create<TSource, TSourceConfiguration>(), async source => await getter((TSource)source), kind));

    void ConfigureSpecificSource<T, TConfiguration, TSource, TSourceConfiguration>(Func<TSource, T> getter, ServiceKind kind = ServiceKind.Singleton)
        where T : notnull
        where TConfiguration : IServiceConfiguration, new()
        where TSource : notnull
        where TSourceConfiguration : IServiceConfiguration, new()
        => ConfigureSpecificSource<T, TConfiguration, TSource, TSourceConfiguration>(source => new ValueTask<T>(getter(source)), kind);

    async ValueTask<object?> TryResolve(ServiceTag tag, Action<IServiceContainer> configure)
    {
        IServiceContainer context = this.DeriveContainer();
        configure(context);
        object? instance = await context.TryResolve(tag);

        if (instance == null)
        {
            await context.DisposeAsync();
            return null;
        }
        else if (instance is ICompositeDisposable composite)
        {
            composite.Add(context);
            return instance;
        }
        else
        {
            await context.DisposeAsync();
            throw new InvalidOperationException($"Type {instance.GetType().Name} must implement ICompositeDisposable");
        }
    }
    
    async ValueTask<object> Resolve(ServiceTag tag, Action<IServiceContainer> configure)
        => await TryResolve(tag, configure) ?? throw new InvalidOperationException();

    async ValueTask<IMaybe<T>> TryResolve<T>(Action<IServiceContainer> configure)
        where T : notnull
        => (await TryResolve(ServiceTag.Create<T>(), configure)).AsNullableMaybe().Cast<T>();

    async ValueTask<T> Resolve<T>(Action<IServiceContainer> configure)
        where T : notnull
        => (await TryResolve<T>(configure)).Value;

    async ValueTask<IMaybe<T>> TryResolve<T, TConfiguration>(Action<IServiceContainer> configure)
        where T : notnull
        where TConfiguration : IServiceConfiguration, new()
        => (await TryResolve(ServiceTag.Create<T, TConfiguration>(), configure)).AsNullableMaybe().Cast<T>();

    async ValueTask<T> Resolve<T, TConfiguration>(Action<IServiceContainer> configure)
        where T : notnull
        where TConfiguration : IServiceConfiguration, new()
        => (await TryResolve<T, TConfiguration>(configure)).Value;
}

public class ServiceContainer : ManagedAsyncDisposable, IServiceContainer
{
    public ServiceContainer(IServiceConfigurationGenerator generator)
    {
        this.generator = generator;

        This.ConfigureInstance<IServiceContainer>(this, false);
    }

    public ServiceContainer()
        : this(new DefaultConfigurationGenerator()) { }

    private readonly IServiceConfigurationGenerator generator;
    private readonly Dictionary<ServiceTag, Entry> entries = new();

    private IServiceContainer This => this;

    public void Configure(ServiceTag tag, IServiceConfiguration configuration)
        => SetEntry(tag, configuration);

    public void Clear(ServiceTag tag)
        => entries.Remove(tag);

    public async ValueTask<object?> TryResolve(ServiceTag tag)
        => TryGetEntry(tag, out Entry? entry) ? await entry.Resolve() : null;

    private Entry SetEntry(ServiceTag tag, IServiceConfiguration configuration)
    {
        if (entries.TryGetValue(tag, out Entry? entry))
        {
            Disposal.Remove(entry);
        }

        entry = new Entry(this, tag, configuration);
        Disposal.Add(entry);
        entries[tag] = entry;
        return entry;
    }

    private bool TryGetEntry(ServiceTag tag, [NotNullWhen(true)] out Entry? entry)
    {
        if (entries.TryGetValue(tag, out entry))
        {
            return true;
        }
        else
        {
            IServiceConfiguration? configuration = generator.Generate(tag);
            if (configuration != null)
            {
                entry = SetEntry(tag, configuration);
                return true;
            }
            else
            {
                entry = null;
                return false;
            }
        }
    }

    private class Entry : ManagedAsyncDisposable
    {
        public Entry(IServiceResolver resolver, ServiceTag tag, IServiceConfiguration configuration)
        {
            this.resolver = resolver;

            Tag = tag;
            Configuration = configuration;

            if (configuration.Kind == ServiceKind.Singleton)
            {
                singleton = new AsyncLazy<object>(Build).DisposeWith(Disposal);
            }
        }

        private readonly IServiceResolver resolver;
        private readonly AsyncLazy<object>? singleton;

        public ServiceTag Tag { get; }
        public IServiceConfiguration Configuration { get; }

        public async ValueTask<object> Resolve()
            => singleton != null ? await singleton.GetValue() : await Build();

        private async ValueTask<object> Build()
            => await Configuration.Builder.Build(resolver);
    }
}