namespace Markwardt;

public record ObjectTag(Type Type, Type? Scheme = null)
{
    public static implicit operator ObjectTag(Type type)
        => new ObjectTag(type);
}