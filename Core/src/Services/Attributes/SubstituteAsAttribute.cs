namespace Markwardt;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Delegate | AttributeTargets.Enum)]
public class SubstituteAsAttribute : BaseSubstituteAttribute
{
    public SubstituteAsAttribute(Type type)
    {
        Type = type;
    }

    public Type Type { get; }

    public override IServiceTag GetSubstitute(Type type)
        => new TypeTag(Type);
}

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Delegate | AttributeTargets.Enum)]
public class SubstituteAsAttribute<T> : SubstituteAsAttribute
    where T : notnull
{
    public SubstituteAsAttribute()
        : base(typeof(T)) { }
}