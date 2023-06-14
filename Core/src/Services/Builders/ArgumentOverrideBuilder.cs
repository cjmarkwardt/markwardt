namespace Markwardt;

public static class ArgumentOverrideBuilderUtils
{
    public static IObjectBuilder OverrideArguments(this IObjectBuilder builder, IObjectArgumentGenerator? generator)
        => generator == null ? builder : new ArgumentOverrideBuilder(builder, generator);
}

public class ArgumentOverrideBuilder : IObjectBuilder
{
    public ArgumentOverrideBuilder(IObjectBuilder builder, IObjectArgumentGenerator generator)
    {
        this.builder = builder;
        this.generator = generator;
    }

    private readonly IObjectBuilder builder;
    private readonly IObjectArgumentGenerator generator;

    public async ValueTask<object> Build(IObjectResolver resolver, Maybe<IObjectArgumentGenerator> arguments = default)
        => await builder.Build(resolver, !arguments.HasValue ? generator.AsMaybe() : arguments.Stack(generator).AsMaybe());
}