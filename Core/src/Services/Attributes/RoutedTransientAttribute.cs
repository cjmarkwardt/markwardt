namespace Markwardt;

[AttributeUsage(AttributeTargets.Interface)]
public class RoutedTransientAttribute : RoutedServiceAttribute
{
    public RoutedTransientAttribute(Type targetService)
        : base(ServiceLifetime.Transient, targetService) { }
}

[AttributeUsage(AttributeTargets.Interface)]
public class RoutedTransientAttribute<TTargetService> : RoutedTransientAttribute
    where TTargetService : class
{
    public RoutedTransientAttribute()
        : base(typeof(TTargetService)) { }
}