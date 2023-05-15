namespace Markwardt;

[AttributeUsage(AttributeTargets.Interface)]
public class RoutedSingletonAttribute : RoutedServiceAttribute
{
    public RoutedSingletonAttribute(Type targetService)
        : base(ServiceLifetime.Singleton, targetService) { }
}

[AttributeUsage(AttributeTargets.Interface)]
public class RoutedSingletonAttribute<TTargetService> : RoutedSingletonAttribute
    where TTargetService : class
{
    public RoutedSingletonAttribute()
        : base(typeof(TTargetService)) { }
}