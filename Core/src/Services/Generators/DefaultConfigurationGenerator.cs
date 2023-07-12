namespace Markwardt;

public class DefaultConfigurationGenerator : IServiceConfigurationGenerator
{
    public IServiceConfiguration? Generate(IServiceTag tag)
    {
        if (tag is ConfigurationTag configurationTag)
        {
            return (IServiceConfiguration)Activator.CreateInstance(configurationTag.Configuration);
        }
        else if (tag is TypeTag typeTag)
        {
            return Scan(typeTag.Type);
        }
        else
        {
            return null;
        }
    }

    private IServiceConfiguration? Scan(Type type)
    {
        if (type.TryGetCustomAttribute(out BaseServiceAttribute? serviceAttribute))
        {
            return serviceAttribute.GetConfiguration(type);
        }
        else if (type.TryGetCustomAttribute(out BaseSubstituteAttribute? substituteAttribute))
        {
            return new SubstituteConfiguration(substituteAttribute.GetSubstitute(type));
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