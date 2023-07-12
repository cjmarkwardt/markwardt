namespace Markwardt;

public class TransientAttribute : ServiceAttribute
{
    public TransientAttribute(Type? implementation = null, Type? arguments = null)
        : base(implementation, OpenServiceKind.Transient, arguments) { }
}

public class TransientAttribute<TImplementation> : TransientAttribute
    where TImplementation : class
{
    public TransientAttribute()
        : base(typeof(TImplementation)) { }
}

public class TransientAttribute<TImplementation, TArguments> : TransientAttribute
    where TImplementation : class
    where TArguments : IServiceArgumentGenerator
{
    public TransientAttribute()
        : base(typeof(TImplementation), typeof(TArguments)) { }
}