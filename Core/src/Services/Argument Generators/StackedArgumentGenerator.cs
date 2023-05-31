namespace Markwardt;

public static class StackedArgumentGeneratorUtils
{
    public static IArgumentGenerator Stack(this IArgumentGenerator generator, IEnumerable<IArgumentGenerator> generators)
        => new StackedArgumentGenerator(generators.Prepend(generator));

    public static IArgumentGenerator Stack(this IArgumentGenerator generator, params IArgumentGenerator[] generators)
        => generator.Stack((IEnumerable<IArgumentGenerator>)generators); 
}

public record StackedArgumentGenerator(IEnumerable<IArgumentGenerator> Generators) : IArgumentGenerator
{
    public async ValueTask Generate(IObjectContainer container, IDictionary<string, object?> arguments)
    {
        foreach (IArgumentGenerator generator in Generators)
        {
            await generator.Generate(container, arguments);
        }
    }
}