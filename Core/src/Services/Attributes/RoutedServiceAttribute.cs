namespace Markwardt;

[AttributeUsage(AttributeTargets.Interface)]
public class RoutedServiceAttribute : ServiceLifetimeAttribute
{
    public RoutedServiceAttribute(ServiceLifetime lifetime, Type targetService)
        : base(lifetime)
    {
        TargetService = targetService;
    }

    public Type TargetService { get; }
}

[AttributeUsage(AttributeTargets.Interface)]
public class RoutedServiceAttribute<TTargetService> : RoutedServiceAttribute
    where TTargetService : class
{
    public RoutedServiceAttribute(ServiceLifetime lifetime)
        : base(lifetime, typeof(TTargetService)) { }
}