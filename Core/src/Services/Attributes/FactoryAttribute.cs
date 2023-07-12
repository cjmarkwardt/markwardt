namespace Markwardt;

[AttributeUsage(AttributeTargets.Delegate)]
public class FactoryAttribute : ServiceAttribute
{
    public FactoryAttribute(Type? implementation = null, string? name = null, Type[]? arguments = null)
        : base(implementation, OpenServiceKind.Singleton, null, null, name, arguments) { }
}

[AttributeUsage(AttributeTargets.Delegate)]
public class FactoryAttribute<T> : FactoryAttribute
{
    public FactoryAttribute(string? name = null, Type[]? arguments = null)
        : base(typeof(T), name, arguments) { }
}