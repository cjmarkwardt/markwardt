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
        generalizer = new(@delegate);
    }

    private readonly IServiceBuilder builder;
    private readonly TypeGeneralizer generalizer;
    private readonly DelegateType type;
    private readonly Dictionary<IValueDictionary<string, Type>, Delegate> delegates = new();

    public async ValueTask<object> Build(IServiceResolver resolver, IServiceArgumentGenerator? argumentGenerator = null)
    {
        IDictionary<string, object?>? arguments = await argumentGenerator.Generate(resolver);
        IValueDictionary<string, Type> typeArguments = generalizer.GetTypeArguments(arguments);
        if (!delegates.TryGetValue(typeArguments, out Delegate? @delegate))
        {
            @delegate = generalizer.Specify(typeArguments).AsDelegate()!.Implement(arguments => builder.Build(resolver, argumentGenerator)!);
            delegates.Add(typeArguments, @delegate);
        }

        return new ValueTask<object>(@delegate);
    }
}