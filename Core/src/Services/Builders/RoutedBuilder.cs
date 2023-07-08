namespace Markwardt;

public class RoutedBuilder : IServiceBuilder
{
    public RoutedBuilder(ServiceTag tag, IServiceResolver? resolver = null)
    {
        this.resolver = resolver;

        Tag = tag;
    }

    private readonly IServiceResolver? resolver;

    public ServiceTag Tag { get; }

    public async ValueTask<object> Build(IServiceResolver resolver, IServiceArgumentGenerator? arguments = null)
        => await (this.resolver ?? resolver).Resolve(Tag);
}