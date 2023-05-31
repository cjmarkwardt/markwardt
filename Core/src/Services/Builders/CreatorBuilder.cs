namespace Markwardt;

public record CreatorBuilder(Type Target) : IObjectBuilder
{
    public async ValueTask<object> Build(IObjectContainer container, IArgumentGenerator? argumentGenerator = null)
        => await container.Create(Target, argumentGenerator);
}