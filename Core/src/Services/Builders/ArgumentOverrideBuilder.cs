namespace Markwardt;

public static class ArgumentOverrideBuilderUtils
{
    public static IServiceBuilder OverrideArguments(this IServiceBuilder builder, IServiceArgumentGenerator? generator)
        => generator == null ? builder : new ArgumentOverrideBuilder(builder, generator);

    public static IServiceBuilder OverrideArguments(this IServiceBuilder builder, IDictionary<string, AsyncFunc<IServiceResolver, object?>>? arguments)
        => arguments == null ? builder : builder.OverrideArguments(new ServiceArgumentGenerator(arguments));

    public static IServiceBuilder OverrideArguments(this IServiceBuilder builder, IDictionary<string, object?>? arguments)
        => arguments == null ? builder : builder.OverrideArguments(new ServiceArgumentGenerator(arguments));

    public static IServiceBuilder OverrideArguments(this IServiceBuilder builder, Type? arguments)
        => arguments == null ? builder : builder.OverrideArguments((IServiceArgumentGenerator)Activator.CreateInstance(arguments));
}

public class ArgumentOverrideBuilder : IServiceBuilder
{
    public ArgumentOverrideBuilder(IServiceBuilder builder, IServiceArgumentGenerator generator)
    {
        this.builder = builder;
        this.generator = generator;
    }

    private readonly IServiceBuilder builder;
    private readonly IServiceArgumentGenerator generator;

    public async ValueTask<object> Build(IServiceResolver resolver, IServiceArgumentGenerator? arguments = null)
        => await builder.Build(resolver, arguments == null ? generator : arguments.Stack(generator));
}