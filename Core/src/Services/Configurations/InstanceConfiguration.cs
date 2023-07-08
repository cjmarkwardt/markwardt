namespace Markwardt;

public class InstanceConfiguration : IServiceConfiguration
{
    public InstanceConfiguration(object instance, bool dispose = true)
    {
        Builder = new FixedBuilder(instance);
        Kind = dispose ? ServiceKind.Singleton : ServiceKind.Transient;
    }

    public IServiceBuilder Builder { get; }
    public ServiceKind Kind { get; }
}