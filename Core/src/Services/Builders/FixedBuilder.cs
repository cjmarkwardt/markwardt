namespace Markwardt;

public class FixedBuilder : IServiceBuilder
{
    public FixedBuilder(object instance)
    {
        this.instance = instance;
    }

    private readonly object instance;

    public ValueTask<object> Build(IServiceResolver resolver, IServiceArgumentGenerator? arguments = null)
        => new ValueTask<object>(instance);
}