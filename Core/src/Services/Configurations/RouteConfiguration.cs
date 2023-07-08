namespace Markwardt;

public class RouteConfiguration : IServiceConfiguration
{
    public RouteConfiguration(ServiceTag target, IServiceResolver? resolver = null)
    {
        Builder = new RoutedBuilder(target, resolver);
    }

    public IServiceBuilder Builder { get; }

    public ServiceKind Kind => ServiceKind.Transient;
}