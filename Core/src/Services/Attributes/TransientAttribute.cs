namespace Markwardt;

public class TransientAttribute : ServiceAttribute
{
    public TransientAttribute(Type? implementation = null, Type? arguments = null)
    {
        Implementation = implementation;
        Arguments = arguments;
    }

    public Type? Implementation { get; }
    public Type? Arguments { get; }

    public override IObjectScheme GetScheme(Type type)
        => new ImplementationScheme(Implementation ?? type.GetInterfaceImplementation())
        {
            Arguments = Arguments.TryCreate<IObjectArgumentGenerator>()
        };
}

public class TransientAttribute<TImplementation> : TransientAttribute
    where TImplementation : class
{
    public TransientAttribute()
        : base(typeof(TImplementation)) { }
}

public class TransientAttribute<TImplementation, TArguments> : TransientAttribute
    where TImplementation : class
    where TArguments : IObjectArgumentGenerator
{
    public TransientAttribute()
        : base(typeof(TImplementation), typeof(TArguments)) { }
}