namespace Markwardt;

public class FixedBuilder : IObjectBuilder
{
    public FixedBuilder(object instance)
    {
        this.instance = instance;
    }

    private readonly object instance;

    public ValueTask<object> Build(IObjectResolver resolver, Maybe<IObjectArgumentGenerator> arguments = default)
        => new ValueTask<object>(instance);
}

public class FixedBuilder<T> : IObjectBuilder<T>
{
    public ValueTask<T> Build(IObjectResolver resolver, Maybe<IObjectArgumentGenerator> arguments = default)
    {
        throw new NotImplementedException();
    }

    ValueTask<object> IObjectBuilder.Build(IObjectResolver resolver, Maybe<IObjectArgumentGenerator> arguments)
    {
        throw new NotImplementedException();
    }
}