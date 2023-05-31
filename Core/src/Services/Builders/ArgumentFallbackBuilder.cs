namespace Markwardt;

public static class ArgumentFallbackBuilderUtils
{
    public static IObjectBuilder FallbackArguments(this IObjectBuilder builder, IArgumentGenerator? generator)
        => generator == null ? builder : new ArgumentFallbackBuilder(builder, generator);
}

public class ArgumentFallbackBuilder : WrappedBuilder
{
    public ArgumentFallbackBuilder(IObjectBuilder builder, IArgumentGenerator generator)
        : base(builder)
    {
        this.generator = generator;
    }

    private readonly IArgumentGenerator generator;

    public override async ValueTask<object> Build(IObjectContainer container, IArgumentGenerator? arguments = null)
        => await Builder.Build(container, arguments == null ? generator : generator.Stack(arguments));
}