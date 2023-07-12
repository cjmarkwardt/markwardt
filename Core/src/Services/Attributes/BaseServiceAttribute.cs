namespace Markwardt;

public abstract class BaseServiceAttribute : Attribute
{
    public abstract IServiceConfiguration GetConfiguration(Type type);
}