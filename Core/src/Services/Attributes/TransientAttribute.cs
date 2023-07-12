namespace Markwardt;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Delegate | AttributeTargets.Enum)]
public class TransientAttribute : ServiceAttribute
{
    public TransientAttribute(Type? implementation = null, Type? arguments = null)
        : base(implementation, OpenServiceKind.Transient, arguments) { }
}

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Delegate | AttributeTargets.Enum)]
public class TransientAttribute<TImplementation> : TransientAttribute
    where TImplementation : class
{
    public TransientAttribute()
        : base(typeof(TImplementation)) { }
}

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Delegate | AttributeTargets.Enum)]
public class TransientAttribute<TImplementation, TArguments> : TransientAttribute
    where TImplementation : class
    where TArguments : IServiceArgumentGenerator
{
    public TransientAttribute()
        : base(typeof(TImplementation), typeof(TArguments)) { }
}