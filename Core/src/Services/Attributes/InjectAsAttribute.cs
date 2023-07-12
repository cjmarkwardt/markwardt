namespace Markwardt;

[AttributeUsage(AttributeTargets.Parameter)]
public class InjectAsAttribute : BaseInjectAttribute
{
    public InjectAsAttribute(Type type)
    {
        Type = type;
    }

    public Type Type { get; }

    public override IServiceTag GetTarget(Type type)
        => new TypeTag(Type);
}

[AttributeUsage(AttributeTargets.Parameter)]
public class InjectAsAttribute<T> : InjectAsAttribute
    where T : notnull
{
    public InjectAsAttribute()
        : base(typeof(T)) { }
}