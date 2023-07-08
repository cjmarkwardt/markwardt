namespace Markwardt;

public static class StackedArgumentGeneratorUtils
{
    public static IServiceArgumentGenerator Stack(this IServiceArgumentGenerator generator, IEnumerable<IServiceArgumentGenerator> generators)
        => new StackedArgumentGenerator(generators.Prepend(generator));

    public static IServiceArgumentGenerator Stack(this IServiceArgumentGenerator generator, params IServiceArgumentGenerator[] generators)
        => generator.Stack((IEnumerable<IServiceArgumentGenerator>)generators);

    public static IServiceArgumentGenerator Stack(this Maybe<IServiceArgumentGenerator> generator, IEnumerable<IServiceArgumentGenerator> generators)
        => new StackedArgumentGenerator(generators.Prepend(generator));

    public static IServiceArgumentGenerator Stack(this Maybe<IServiceArgumentGenerator> generator, params IServiceArgumentGenerator[] generators)
        => generator.Stack((IEnumerable<IServiceArgumentGenerator>)generators);
}

public record StackedArgumentGenerator(IEnumerable<IServiceArgumentGenerator> Generators) : IServiceArgumentGenerator
{
    public async ValueTask Generate(IServiceResolver resolver, IDictionary<string, object?> arguments)
    {
        foreach (IServiceArgumentGenerator generator in Generators)
        {
            await generator.Generate(resolver, arguments);
        }
    }
}