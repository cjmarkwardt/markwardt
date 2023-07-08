namespace Markwardt;

public abstract class ServiceAttribute : Attribute
{
    public abstract IServiceConfiguration GetConfiguration(Type type);
}