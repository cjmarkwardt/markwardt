namespace Markwardt;

public class MethodTargetBuilder : IObjectBuilder
{
    public MethodTargetBuilder(MethodBase method)
    {
        this.method = method;
        genericTargeter = new(method);
    }

    private readonly MethodBase method;
    private readonly GenericMethodTargeter genericTargeter;
    private readonly Dictionary<IValueDictionary<string, Type>, MethodInvoker> invokers = new();

    public async ValueTask<object> Build(IObjectContainer container, IArgumentGenerator? argumentGenerator = null)
    {
        IDictionary<string, object?>? arguments = null;
        if (argumentGenerator != null)
        {
            arguments = await argumentGenerator.Generate(container);
        }

        IValueDictionary<string, Type> typeArguments = genericTargeter.GetTypeArguments(arguments);
        if (!invokers.TryGetValue(typeArguments, out MethodInvoker? invoker))
        {
            invoker = new MethodInvoker(genericTargeter.Close(typeArguments));
            invokers.Add(typeArguments, invoker);
        }

        return await invoker.Invoke(container, arguments);
    }
}