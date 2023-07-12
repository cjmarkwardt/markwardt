namespace Markwardt;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Delegate | AttributeTargets.Enum)]
public class SubstituteAttribute : BaseSubstituteAttribute
{
    public SubstituteAttribute(Type configuration)
    {
        Configuration = configuration;
    }

    public Type Configuration { get; }

    public override IServiceTag GetSubstitute(Type type)
        => new ConfigurationTag(Configuration);
}

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Delegate | AttributeTargets.Enum)]
public class SubstituteAttribute<TConfiguration> : SubstituteAsAttribute
    where TConfiguration : IServiceConfiguration, new()
{
    public SubstituteAttribute()
        : base(typeof(TConfiguration)) { }
}