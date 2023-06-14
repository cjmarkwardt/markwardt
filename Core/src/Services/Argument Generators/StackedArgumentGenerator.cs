namespace Markwardt;

public static class StackedArgumentGeneratorUtils
{
    public static IObjectArgumentGenerator Stack(this IObjectArgumentGenerator generator, IEnumerable<IObjectArgumentGenerator> generators)
        => new StackedArgumentGenerator(generators.Prepend(generator));

    public static IObjectArgumentGenerator Stack(this IObjectArgumentGenerator generator, params IObjectArgumentGenerator[] generators)
        => generator.Stack((IEnumerable<IObjectArgumentGenerator>)generators);

    public static IObjectArgumentGenerator Stack(this Maybe<IObjectArgumentGenerator> generator, IEnumerable<IObjectArgumentGenerator> generators)
        => new StackedArgumentGenerator(generators.Prepend(generator));

    public static IObjectArgumentGenerator Stack(this Maybe<IObjectArgumentGenerator> generator, params IObjectArgumentGenerator[] generators)
        => generator.Stack((IEnumerable<IObjectArgumentGenerator>)generators);
}

public record StackedArgumentGenerator(IEnumerable<IObjectArgumentGenerator> Generators) : IObjectArgumentGenerator
{
    public async ValueTask Generate(IObjectResolver resolver, IDictionary<string, object?> arguments)
    {
        foreach (IObjectArgumentGenerator generator in Generators)
        {
            await generator.Generate(resolver, arguments);
        }
    }
}