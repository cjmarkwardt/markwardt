namespace Markwardt;

public abstract class ServiceAttribute : Attribute
{
    public abstract IObjectScheme GetScheme(Type type);
}