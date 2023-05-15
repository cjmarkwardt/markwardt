namespace Markwardt;

public static class MappedArgumentsBuilderUtils
{
    public static IServiceBuilder WithMappedArguments(this IServiceBuilder source, IEnumerable<ArgumentMapping>? mappings)
        => mappings == null || !mappings.Any() ? source : new MappedArgumentsBuilder(source, mappings);
}

public record MappedArgumentsBuilder(IServiceBuilder Source, IEnumerable<ArgumentMapping> Mappings) : IServiceBuilder
{
    public async ValueTask<object> Build(IServiceContainer container, IEnumerable<Argument<object?>>? arguments = null, IEnumerable<Argument<Type>>? typeArguments = null)
        => await Source.Build(container, Mappings.Map(arguments), typeArguments);
}