namespace Markwardt;

public record ResolutionBuilder(ObjectTag Tag) : IObjectBuilder
{
    public async ValueTask<object> Build(IObjectContainer container, Maybe<IObjectArgumentGenerator> arguments = default)
        => await container.Resolve(Tag);
}