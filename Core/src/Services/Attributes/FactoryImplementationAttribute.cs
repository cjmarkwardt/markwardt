namespace Markwardt;

[AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Method)]
public class FactoryImplementationAttribute : Attribute
{
    public FactoryImplementationAttribute(string? id = null)
    {
        Id = id;
    }

    public string? Id { get; }
}