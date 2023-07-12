namespace Markwardt;

public abstract class BaseSubstituteAttribute : Attribute
{
    public abstract IServiceTag GetSubstitute(Type type);
}