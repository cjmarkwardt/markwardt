namespace Markwardt;

public record ResolutionBuilder(ObjectTag Tag) : IObjectBuilder
{
    public async ValueTask<object> Build(IObjectResolver resolver, Maybe<IObjectArgumentGenerator> arguments = default)
        => await resolver.Resolve(Tag);
}