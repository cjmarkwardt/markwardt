namespace Markwardt;

public static class FallbackTypeArgumentsBuilderUtils
{
    public static IServiceBuilder WithFallbackTypeArguments(this IServiceBuilder source, IEnumerable<Argument<Type>>? arguments)
        => arguments == null || !arguments.Any() ? source : new FallbackTypeArgumentsBuilder(source, arguments);
}

public class FallbackTypeArgumentsBuilder : IServiceBuilder
{   
    public FallbackTypeArgumentsBuilder(IServiceBuilder source, IEnumerable<Argument<Type>> fallbackArguments)
    {
        this.source = source;
        this.fallbackArguments = fallbackArguments;
    }

    private readonly IServiceBuilder source;
    private readonly IEnumerable<Argument<Type>> fallbackArguments;

    public async ValueTask<object> Build(IServiceContainer container, IEnumerable<Argument<object?>>? arguments = null, IEnumerable<Argument<Type>>? typeArguments = null)
        => await source.Build(container, arguments, fallbackArguments.Replace(typeArguments));
}