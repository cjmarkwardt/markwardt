namespace Markwardt;

public class InjectAttribute : Attribute
{
    public InjectAttribute(Type configuration)
    {
        Configuration = configuration;
    }

    public Type Configuration { get; }
}

public class InjectAttribute<TConfiguration> : InjectAttribute
    where TConfiguration : IServiceConfiguration, new()
{
    public InjectAttribute()
        : base(typeof(TConfiguration)) { }
}