namespace Markwardt;

public record ResolvedArgumentGenerator(Type GeneratorType) : IArgumentGenerator
{
    public async ValueTask Generate(IObjectContainer container, IDictionary<string, object?> arguments)
        => await ((IArgumentGenerator) await container.Resolve(GeneratorType)).Generate(container, arguments);
}
