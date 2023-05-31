namespace Markwardt;

public class InstantiationBuilder : IObjectBuilder
{
    public InstantiationBuilder(Type target, IMethodLocator? constructLocator = null)
    {
        if (target.IsInstantiable())
        {
            throw new InvalidOperationException($"Type {target} must be a class");
        }

        this.@class = target;
        this.constructLocator = constructLocator ?? new ConstructLocator();
        genericTargeter = new(target);
    }

    private readonly Type @class;
    private readonly IMethodLocator constructLocator;
    private readonly GenericTypeTargeter genericTargeter;
    private readonly Dictionary<IValueDictionary<string, Type>, MethodTargetBuilder> methodBuilders = new();

    public async ValueTask<object> Build(IObjectContainer container, IArgumentGenerator? argumentGenerator = null)
    {
        IDictionary<string, object?>? arguments = null;
        if (argumentGenerator != null)
        {
            arguments = await argumentGenerator.Generate(container);
        }

        IValueDictionary<string, Type> typeArguments = genericTargeter.GetTypeArguments(arguments);
        if (!methodBuilders.TryGetValue(typeArguments, out MethodTargetBuilder? methodBuilder))
        {
            methodBuilder = new MethodTargetBuilder(constructLocator.Locate(genericTargeter.Close(typeArguments)).NotNull());
            methodBuilders.Add(typeArguments, methodBuilder);
        }

        return await methodBuilder.Build(container, arguments == null ? null : ArgumentGenerator.Create(arguments));
    }
}