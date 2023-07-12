namespace Markwardt;

public class DefaultConfigurationGenerator : IServiceConfigurationGenerator
{
    public IServiceConfiguration? Generate(ServiceTag tag)
    {
        if (tag.Configuration != null)
        {
            return (IServiceConfiguration)Activator.CreateInstance(tag.Configuration);
        }
        else
        {
            return Scan(tag.Type);
        }
    }

    private IServiceConfiguration? Scan(Type type)
    {
        if (type.TryGetCustomAttribute(out IServiceAttribute? serviceAttribute))
        {
            return serviceAttribute.GetConfiguration(type);
        }
        else if (type.TryGetCustomAttribute(out RoutedServiceAttribute? routedServiceAttribute))
        {
            return new RouteConfiguration(routedServiceAttribute.Target);
        }
        else if (NaturalConfiguration.TryGet(type, out IServiceConfiguration? naturalConfiguration))
        {
            return naturalConfiguration;
        }
        else
        {
            return null;
        }
    }
}