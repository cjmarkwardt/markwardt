namespace Markwardt;

public class SingletonAttribute : ServiceAttribute
{
    public SingletonAttribute(Type? implementation = null, Type? arguments = null, Type? transientArguments = null)
    {
        Implementation = implementation;
        Arguments = arguments;
        TransientArguments = transientArguments;
    }

    public Type? Implementation { get; }
    public Type? Arguments { get; }
    public Type? TransientArguments { get; }

    public override IObjectScheme GetScheme(Type type)
        => new ImplementationScheme(Implementation ?? type.GetInterfaceImplementation())
        {
            IsSingleton = true,
            SingletonArguments = Arguments.TryCreate<IObjectArgumentGenerator>(),
            Arguments = TransientArguments.TryCreate<IObjectArgumentGenerator>()
        };
}

public class SingletonAttribute<TImplementation> : SingletonAttribute
    where TImplementation : class
{
    public SingletonAttribute()
        : base(typeof(TImplementation)) { }
}

public class SingletonAttribute<TImplementation, TArguments> : SingletonAttribute
    where TImplementation : class
    where TArguments : IObjectArgumentGenerator
{
    public SingletonAttribute()
        : base(typeof(TImplementation), typeof(TArguments)) { }
}

public class SingletonAttribute<TImplementation, TArguments, TTransientArguments> : SingletonAttribute
    where TImplementation : class
    where TArguments : IObjectArgumentGenerator
    where TTransientArguments : IObjectArgumentGenerator
{
    public SingletonAttribute()
        : base(typeof(TImplementation), typeof(TArguments), typeof(TTransientArguments)) { }
}