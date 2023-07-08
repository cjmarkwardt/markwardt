namespace Markwardt;

public interface IServiceConfigurationGenerator
{
    IServiceConfiguration? Generate(ServiceTag tag);
}