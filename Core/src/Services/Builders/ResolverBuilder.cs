namespace Markwardt;

public record ResolverBuilder(Type Target) : IObjectBuilder
{
    public async ValueTask<object> Build(IObjectContainer container, IArgumentGenerator? argumentGenerator = null)
        => await container.Resolve(Target);
}