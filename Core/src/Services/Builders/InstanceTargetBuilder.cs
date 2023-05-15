namespace Markwardt;

public record InstanceTargetBuilder(object Instance) : IServiceBuilder
{
    public ValueTask<object> Build(IServiceContainer container, IEnumerable<Argument<object?>>? arguments = null, IEnumerable<Argument<Type>>? typeArguments = null)
        => new ValueTask<object>(Instance);
}