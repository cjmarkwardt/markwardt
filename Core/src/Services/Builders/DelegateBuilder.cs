namespace Markwardt;

public static class DelegateBuilderUtils
{
    public static IServiceBuilder ThroughDelegate(this IServiceBuilder builder, Type @delegate)
        => !@delegate.IsDelegate() ? builder : new DelegateBuilder(builder, @delegate);
}

public class DelegateBuilder : IServiceBuilder
{
    public DelegateBuilder(IServiceBuilder builder, Type @delegate)
    {
        this.builder = builder;

        if (!@delegate.AsDelegate(out DelegateType? type))
        {
            throw new InvalidOperationException($"Type {@delegate} is not a delegate");
        }
        else if (type.Return.TryGetGenericTypeDefinition() != typeof(ValueTask<>))
        {
            throw new InvalidOperationException($"Delegate {@delegate} must return a ValueTask<T>");
        }

        this.type = type;
    }

    private readonly IServiceBuilder builder;
    private readonly DelegateType type;
    private readonly Dictionary<IValueDictionary<string, Type>, Delegate> delegates = new();

    public ValueTask<object> Build(IServiceContainer container, IEnumerable<Argument<object?>>? arguments = null, IEnumerable<Argument<Type>>? typeArguments = null)
    {
        IValueDictionary<string, Type> genericId = typeArguments.ToValueDictionary(a => a.Name, a => a.Value);
        if (!delegates.TryGetValue(genericId, out Delegate? @delegate))
        {
            @delegate = type.Close(typeArguments).Implement(arguments => builder.Build(container, arguments, typeArguments)!);
            delegates.Add(genericId, @delegate);
        }

        return new ValueTask<object>(@delegate);
    }
}