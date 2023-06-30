namespace Markwardt;

public static class DelegateBuilderUtils
{
    public static IObjectBuilder ThroughDelegate(this IObjectBuilder builder, Type @delegate)
        => !@delegate.IsDelegate() ? builder : new DelegateBuilder(builder, @delegate);
}

public class DelegateBuilder : IObjectBuilder
{
    public DelegateBuilder(IObjectBuilder builder, Type @delegate)
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
        generalizer = new(@delegate);
    }

    private readonly IObjectBuilder builder;
    private readonly TypeGeneralizer generalizer;
    private readonly DelegateType type;
    private readonly Dictionary<IValueDictionary<string, Type>, Delegate> delegates = new();

    public async ValueTask<object> Build(IObjectContainer container, Maybe<IObjectArgumentGenerator> argumentGenerator = default)
    {
        Maybe<IDictionary<string, object?>> arguments = await argumentGenerator.Generate(container);
        IValueDictionary<string, Type> typeArguments = generalizer.GetTypeArguments(arguments);
        if (!delegates.TryGetValue(typeArguments, out Delegate? invoker))
        {
            invoker = generalizer.Specify(typeArguments).implem;
            invokers.Add(typeArguments, invoker);
        }

        return await invoker.Build(resolver, argumentGenerator);

        IDictionary<string, object?>? arguments = null;
        if (argumentGenerator != null)
        {
            arguments = await argumentGenerator.Generate(container);
        }

        IValueDictionary<string, Type> typeArguments = generalizer.GetTypeArguments(arguments);
        if (!delegates.TryGetValue(typeArguments, out Delegate? @delegate))
        {
            @delegate = type.Close(typeArguments).Implement(arguments => builder.Build(container, argumentGenerator)!);
            delegates.Add(typeArguments, @delegate);
        }

        return new ValueTask<object>(@delegate);
    }
}