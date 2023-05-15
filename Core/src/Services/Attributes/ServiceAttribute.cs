namespace Markwardt;

[AttributeUsage(AttributeTargets.Interface | AttributeTargets.Delegate)]
public class ServiceAttribute : ServiceLifetimeAttribute
{
    public ServiceAttribute(ServiceLifetime lifetime, Type implementation)
        : base(lifetime)
    {
        Implementation = implementation;
    }

    public Type Implementation { get; }
}

[AttributeUsage(AttributeTargets.Interface | AttributeTargets.Delegate)]
public class ServiceAttribute<TImplementation> : ServiceAttribute
    where TImplementation : class
{
    public ServiceAttribute(ServiceLifetime lifetime)
        : base(lifetime, typeof(TImplementation)) { }
}