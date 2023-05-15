namespace Markwardt;

public static class FallbackArgumentsBuilderUtils
{
    public static IServiceBuilder WithFallbackArguments(this IServiceBuilder source, IEnumerable<Argument<object?>>? arguments)
        => arguments == null || !arguments.Any() ? source : new FallbackArgumentsBuilder(source, arguments);
}

public class FallbackArgumentsBuilder : IServiceBuilder
{   
    public FallbackArgumentsBuilder(IServiceBuilder source, IEnumerable<Argument<object?>> fallbackArguments)
    {
        this.source = source;
        this.fallbackArguments = fallbackArguments;
    }

    private readonly IServiceBuilder source;
    private readonly IEnumerable<Argument<object?>> fallbackArguments;

    public async ValueTask<object> Build(IServiceContainer container, IEnumerable<Argument<object?>>? arguments = null, IEnumerable<Argument<Type>>? typeArguments = null)
        => await source.Build(container, fallbackArguments.Replace(arguments), typeArguments);
}