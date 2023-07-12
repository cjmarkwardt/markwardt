namespace Markwardt;

[AttributeUsage(AttributeTargets.Parameter)]
public class InjectAttribute : BaseInjectAttribute
{
    public InjectAttribute(Type configuration)
    {
        Configuration = configuration;
    }

    public Type Configuration { get; }

    public override IServiceTag GetTarget(Type type)
        => new ConfigurationTag(Configuration);
}

[AttributeUsage(AttributeTargets.Parameter)]
public class InjectAttribute<TConfiguration> : InjectAttribute
    where TConfiguration : IServiceConfiguration, new()
{
    public InjectAttribute()
        : base(typeof(TConfiguration)) { }
}