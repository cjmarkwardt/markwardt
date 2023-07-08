namespace Markwardt;

public class RoutedServiceAttribute : Attribute
{
    public RoutedServiceAttribute(Type type, Type? configuration = null)
    {
        Target = new ServiceTag(type, configuration);
    }

    public ServiceTag Target { get; }
}

public class RoutedServiceAttribute<TTarget> : RoutedServiceAttribute
{
    public RoutedServiceAttribute()
        : base(typeof(TTarget)) { }
}

public class RoutedServiceAttribute<TTarget, TConfiguration> : RoutedServiceAttribute
{
    public RoutedServiceAttribute()
        : base(typeof(TTarget), typeof(TConfiguration)) { }
}