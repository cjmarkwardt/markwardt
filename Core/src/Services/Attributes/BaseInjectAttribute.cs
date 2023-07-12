namespace Markwardt;

public abstract class BaseInjectAttribute : Attribute
{
    public abstract IServiceTag GetTarget(Type type);
}