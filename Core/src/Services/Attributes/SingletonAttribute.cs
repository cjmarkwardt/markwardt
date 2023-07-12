namespace Markwardt;

public class SingletonAttribute : ServiceAttribute
{
    public SingletonAttribute(Type? implementation = null, Type? arguments = null)
        : base(implementation, OpenServiceKind.Singleton, arguments) { }
}

public class SingletonAttribute<TImplementation> : SingletonAttribute
    where TImplementation : class
{
    public SingletonAttribute()
        : base(typeof(TImplementation)) { }
}

public class SingletonAttribute<TImplementation, TArguments> : SingletonAttribute
    where TImplementation : class
    where TArguments : IServiceArgumentGenerator
{
    public SingletonAttribute()
        : base(typeof(TImplementation), typeof(TArguments)) { }
}