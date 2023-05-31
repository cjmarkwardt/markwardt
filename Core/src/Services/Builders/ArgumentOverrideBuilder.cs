namespace Markwardt;

public static class ArgumentOverrideBuilderUtils
{
    public static IObjectBuilder OverrideArguments(this IObjectBuilder builder, IArgumentGenerator? generator)
        => generator == null ? builder : new ArgumentOverrideBuilder(builder, generator);
}

public class ArgumentOverrideBuilder : WrappedBuilder
{
    public ArgumentOverrideBuilder(IObjectBuilder builder, IArgumentGenerator generator)
        : base(builder)
    {
        this.generator = generator;
    }

    private readonly IArgumentGenerator generator;

    public override async ValueTask<object> Build(IObjectContainer container, IArgumentGenerator? arguments = null)
        => await Builder.Build(container, arguments == null ? generator : arguments.Stack(generator));
}