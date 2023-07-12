namespace Markwardt;

public class InstantiationBuilder : IServiceBuilder
{
    public InstantiationBuilder(Type target, Func<Type, MethodBase?>? methodLocator = null)
    {
        if (!target.IsInstantiable())
        {
            throw new InvalidOperationException($"Type {target} must be instantiable");
        }

        this.@class = target;
        this.methodLocator = methodLocator ?? LocateMethod;
        generalizer = new(target);
    }

    private readonly Type @class;
    private readonly Func<Type, MethodBase?> methodLocator;
    private readonly TypeGeneralizer generalizer;
    private readonly Dictionary<IValueDictionary<string, Type>, InvocationBuilder> invokers = new();

    public async ValueTask<object> Build(IServiceResolver resolver, IServiceArgumentGenerator? argumentGenerator = null)
    {
        IDictionary<string, object?>? arguments = await argumentGenerator.Generate(resolver);
        IValueDictionary<string, Type> typeArguments = generalizer.GetTypeArguments(arguments);
        if (!invokers.TryGetValue(typeArguments, out InvocationBuilder? invoker))
        {
            invoker = new InvocationBuilder(methodLocator(generalizer.Specify(typeArguments)).NotNull());
            invokers.Add(typeArguments, invoker);
        }

        return await invoker.Build(resolver, argumentGenerator);
    }

    private MethodBase? LocateMethod(Type type)
    {
        bool IsCopyConstructor(ConstructorInfo constructor)
            => constructor.GetParameters().Length == 1 && constructor.GetParameters().First().ParameterType == type;

        IEnumerable<MethodBase> methods = type.GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).Where(c => !IsCopyConstructor(c)).Cast<MethodBase>().Concat(type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic));

        MethodBase? attributed = methods.FirstOrDefault(m => m.GetCustomAttribute<FactoryImplementationAttribute>() != null);
        if (attributed != null)
        {
            return attributed;
        }

        MethodBase? staticCreate = methods.OfType<MethodInfo>().Where(m => m.Name == "Create").OrderByDescending(m => m.GetParameters().Length).FirstOrDefault();
        if (staticCreate != null)
        {
            return staticCreate;
        }

        MethodBase? longestConstructor = methods.OfType<ConstructorInfo>().OrderByDescending(m => m.GetParameters().Length).FirstOrDefault();
        if (longestConstructor != null)
        {
            return longestConstructor;
        }

        return null;
    }
}
