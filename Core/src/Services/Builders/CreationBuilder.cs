namespace Markwardt;

public record CreationBuilder(ObjectTag Tag) : IObjectBuilder
{
    public async ValueTask<object> Build(IObjectContainer container, Maybe<IObjectArgumentGenerator> arguments = default)
        => await container.Create(Tag, arguments);
}