namespace Markwardt;

[AttributeUsage(AttributeTargets.Interface | AttributeTargets.Delegate)]
public class SingletonAttribute : ServiceAttribute
{
    public SingletonAttribute(Type implementation)
        : base(ServiceLifetime.Singleton, implementation) { }
}

[AttributeUsage(AttributeTargets.Interface | AttributeTargets.Delegate)]
public class SingletonAttribute<TImplementation> : SingletonAttribute
    where TImplementation : class
{
    public SingletonAttribute()
        : base(typeof(TImplementation)) { }
}