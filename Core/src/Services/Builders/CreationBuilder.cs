namespace Markwardt;

public record CreationBuilder(ObjectTag Tag) : IObjectBuilder
{
    public async ValueTask<object> Build(IObjectResolver resolver, Maybe<IObjectArgumentGenerator> arguments = default)
        => await resolver.Create(Tag, arguments);
}