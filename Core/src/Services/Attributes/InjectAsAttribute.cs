namespace Markwardt;

public class InjectAsAttribute : Attribute
{
    public InjectAsAttribute(ServiceTag tag)
    {
        Tag = tag;
    }

    public InjectAsAttribute(Type type, Type? configuration = null)
        : this(new ServiceTag(type, configuration)) { }

    public ServiceTag Tag { get; }
}

public class InjectAsAttribute<T> : InjectAsAttribute
    where T : notnull
{
    public InjectAsAttribute()
        : base(ServiceTag.Create<T>()) { }
}

public class InjectAsAttribute<T, TConfiguration> : InjectAsAttribute
    where T : notnull
    where TConfiguration : IServiceConfiguration, new()
{
    public InjectAsAttribute()
        : base(ServiceTag.Create<T, TConfiguration>()) { }
}