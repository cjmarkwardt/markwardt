namespace Markwardt;

public interface IServiceFactory
{
    ValueTask<object> Create(IServiceResolver resolver, IDictionary<string, object?>? arguments = null);
}

public interface IServiceFactory<T>
{
    IServiceFactory Generalize();
    ValueTask<T> Create(IServiceResolver resolver, IDictionary<string, object?>? arguments = null);
}

public record ServiceFactory(AsyncFunc<IServiceResolver, IDictionary<string, object?>?, object> Delegate) : IServiceFactory
{
    public ServiceFactory(AsyncFunc<IServiceResolver, object> Delegate)
        : this(async (resolver, _) => await Delegate(resolver)) { }
    
    public ServiceFactory(AsyncFunc<object> Delegate)
        : this(async (_, _) => await Delegate()) { }
    
    public ServiceFactory(Func<IServiceResolver, IDictionary<string, object?>?, object> Delegate)
        : this((resolver, arguments) => new ValueTask<object>(Delegate(resolver, arguments))) { }
    
    public ServiceFactory(Func<IServiceResolver, object> Delegate)
        : this((resolver, _) => new ValueTask<object>(Delegate(resolver))) { }
    
    public ServiceFactory(Func<object> Delegate)
        : this((_, _) => new ValueTask<object>(Delegate())) { }

    public async ValueTask<object> Create(IServiceResolver resolver, IDictionary<string, object?>? arguments = null)
        => await Delegate(resolver, arguments);
}

public record ServiceFactory<T>(AsyncFunc<IServiceResolver, IDictionary<string, object?>?, T> Delegate) : IServiceFactory<T>
    where T : notnull
{
    public ServiceFactory(AsyncFunc<IServiceResolver, T> Delegate)
        : this(async (resolver, _) => await Delegate(resolver)) { }
    
    public ServiceFactory(AsyncFunc<T> Delegate)
        : this(async (_, _) => await Delegate()) { }
    
    public ServiceFactory(Func<IServiceResolver, IDictionary<string, object?>?, T> Delegate)
        : this((resolver, arguments) => new ValueTask<T>(Delegate(resolver, arguments))) { }
    
    public ServiceFactory(Func<IServiceResolver, T> Delegate)
        : this((resolver, _) => new ValueTask<T>(Delegate(resolver))) { }
    
    public ServiceFactory(Func<T> Delegate)
        : this((_, _) => new ValueTask<T>(Delegate())) { }

    public IServiceFactory Generalize()
        => new ServiceFactory(async (resolver, arguments) => await Delegate(resolver, arguments));

    public async ValueTask<T> Create(IServiceResolver resolver, IDictionary<string, object?>? arguments = null)
        => await Delegate(resolver, arguments);
}