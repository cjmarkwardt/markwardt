namespace Markwardt;

public class ImplementationAttribute : Attribute
{
    public ImplementationAttribute(Type implementation, Type? arguments = null)
    {
        Implementation = implementation;
        Arguments = arguments;
    }

    public Type Implementation { get; }
    public Type? Arguments { get; }
}

public class ImplementationAttribute<TImplementation> : ImplementationAttribute
    where TImplementation : class
{
    public ImplementationAttribute()
        : base(typeof(TImplementation)) { }
}

public class ImplementationAttribute<TImplementation, TArguments> : ImplementationAttribute
    where TImplementation : class
    where TArguments : IArgumentGenerator
{
    public ImplementationAttribute()
        : base(typeof(TImplementation), typeof(TArguments)) { }
}