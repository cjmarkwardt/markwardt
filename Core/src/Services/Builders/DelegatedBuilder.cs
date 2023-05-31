namespace Markwardt;

public record DelegatedBuilder(BuildFunction Delegate) : IObjectBuilder
{
    public async ValueTask<object> Build(IObjectContainer container, IArgumentGenerator? argumentGenerator = null)
        => await Delegate(container, argumentGenerator);
}