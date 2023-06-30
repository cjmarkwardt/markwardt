namespace Markwardt;

public class FixedBuilder : IObjectBuilder
{
    public FixedBuilder(object instance)
    {
        this.instance = instance;
    }

    private readonly object instance;

    public ValueTask<object> Build(IObjectContainer container, Maybe<IObjectArgumentGenerator> arguments = default)
        => new ValueTask<object>(instance);
}