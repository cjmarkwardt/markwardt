namespace Markwardt;

public class ResolverConfigurationGenerator : IServiceConfigurationGenerator
{
    public ResolverConfigurationGenerator(IServiceResolver resolver)
    {
        this.resolver = resolver;
    }

    private readonly IServiceResolver resolver;

    public IServiceConfiguration? Generate(IServiceTag tag)
        => new RoutedBuilder(tag, resolver).AsTransient();
}