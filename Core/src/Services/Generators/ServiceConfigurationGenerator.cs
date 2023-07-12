namespace Markwardt;

public interface IServiceConfigurationGenerator
{
    IServiceConfiguration? Generate(IServiceTag tag);
}