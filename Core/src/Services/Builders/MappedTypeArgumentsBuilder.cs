namespace Markwardt;

public static class MappedTypeArgumentsBuilderUtils
{
    public static IServiceBuilder WithMappedTypeArguments(this IServiceBuilder source, IEnumerable<ArgumentMapping>? mappings)
        => mappings == null || !mappings.Any() ? source : new MappedTypeArgumentsBuilder(source, mappings);
}

public record MappedTypeArgumentsBuilder(IServiceBuilder Source, IEnumerable<ArgumentMapping> Mappings) : IServiceBuilder
{
    public async ValueTask<object> Build(IServiceContainer container, IEnumerable<Argument<object?>>? arguments = null, IEnumerable<Argument<Type>>? typeArguments = null)
        => await Source.Build(container, arguments, Mappings.Map(typeArguments));
}