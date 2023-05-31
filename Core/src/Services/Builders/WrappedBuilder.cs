namespace Markwardt;

public abstract class WrappedBuilder : IObjectBuilder
{
    public WrappedBuilder(IObjectBuilder builder)
    {
        Builder = builder;
    }

    protected IObjectBuilder Builder { get; }

    public abstract ValueTask<object> Build(IObjectContainer container, IArgumentGenerator? arguments = null);
}