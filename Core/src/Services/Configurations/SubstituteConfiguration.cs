namespace Markwardt;

public class SubstituteConfiguration : IServiceConfiguration
{
    public SubstituteConfiguration(IServiceTag target, IServiceResolver? resolver = null)
    {
        Builder = new RoutedBuilder(target, resolver);
    }

    public IServiceBuilder Builder { get; }

    public ServiceKind Kind => ServiceKind.Transient;
}