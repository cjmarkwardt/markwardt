namespace Markwardt;

[AttributeUsage(AttributeTargets.Delegate)]
public class TargetFactoryAttribute : ServiceAttribute
{
    public TargetFactoryAttribute(string id, Type? implementation = null)
        : base(implementation, OpenServiceKind.Singleton, null, id) { }
}

[AttributeUsage(AttributeTargets.Delegate)]
public class TargetFactoryAttribute<T> : TargetFactoryAttribute
{
    public TargetFactoryAttribute(string id)
        : base(id, typeof(T)) { }
}