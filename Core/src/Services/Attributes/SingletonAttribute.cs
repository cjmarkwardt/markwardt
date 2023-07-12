namespace Markwardt;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Delegate | AttributeTargets.Enum)]
public class SingletonAttribute : ServiceAttribute
{
    public SingletonAttribute(Type? implementation = null, Type? arguments = null)
        : base(implementation, OpenServiceKind.Singleton, arguments) { }
}

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Delegate | AttributeTargets.Enum)]
public class SingletonAttribute<TImplementation> : SingletonAttribute
    where TImplementation : class
{
    public SingletonAttribute()
        : base(typeof(TImplementation)) { }
}

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Delegate | AttributeTargets.Enum)]
public class SingletonAttribute<TImplementation, TArguments> : SingletonAttribute
    where TImplementation : class
    where TArguments : IServiceArgumentGenerator
{
    public SingletonAttribute()
        : base(typeof(TImplementation), typeof(TArguments)) { }
}