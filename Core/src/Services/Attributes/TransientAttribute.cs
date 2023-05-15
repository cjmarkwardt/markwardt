namespace Markwardt;

[AttributeUsage(AttributeTargets.Interface | AttributeTargets.Delegate)]
public class TransientAttribute : ServiceAttribute
{
    public TransientAttribute(Type implementation)
        : base(ServiceLifetime.Transient, implementation) { }
}

[AttributeUsage(AttributeTargets.Interface | AttributeTargets.Delegate)]
public class TransientAttribute<TImplementation> : TransientAttribute
    where TImplementation : class
{
    public TransientAttribute()
        : base(typeof(TImplementation)) { }
}