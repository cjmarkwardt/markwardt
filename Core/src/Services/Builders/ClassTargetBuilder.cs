namespace Markwardt;

public class ClassTargetBuilder : IServiceBuilder
{
    public static async ValueTask<object> Build(Type @class, IServiceContainer container, IEnumerable<Argument<object?>>? arguments = null, IEnumerable<Argument<Type>>? typeArguments = null)
        => await (new ClassTargetBuilder(@class).Build(container, arguments, typeArguments));

    public static async ValueTask<T> Build<T>(IServiceContainer container, IEnumerable<Argument<object?>>? arguments = null, IEnumerable<Argument<Type>>? typeArguments = null)
        => (T) await Build(typeof(T), container, arguments, typeArguments);

    public ClassTargetBuilder(Type @class)
    {
        if (!@class.IsClass)
        {
            throw new InvalidOperationException($"Type {@class} must be a class");
        }

        this.@class = @class;
    }

    private readonly Type @class;
    private readonly Dictionary<IValueDictionary<string, Type>, MethodTargetBuilder> methodBuilders = new();

    public async ValueTask<object> Build(IServiceContainer container, IEnumerable<Argument<object?>>? arguments = null, IEnumerable<Argument<Type>>? typeArguments = null)
    {
        IValueDictionary<string, Type> typeArgumentsValue = typeArguments.ToValueDictionary(a => a.Name, a => a.Value);
        if (!methodBuilders.TryGetValue(typeArgumentsValue, out MethodTargetBuilder? methodBuilder))
        {
            methodBuilder = new MethodTargetBuilder(FindMethod(CloseClass(@class, typeArgumentsValue)));
            methodBuilders.Add(typeArgumentsValue, methodBuilder);
        }

        return await methodBuilder.Build(container, arguments, typeArguments);
    }

    protected virtual MethodBase FindMethod(Type type)
    {
        bool IsCopyConstructor(ConstructorInfo constructor)
            => constructor.GetParameters().Length == 1 && constructor.GetParameters().First().ParameterType == type;

        IEnumerable<MethodBase> methods = type.GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).Where(c => !IsCopyConstructor(c)).Cast<MethodBase>().Concat(type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic));

        MethodBase? attributed = methods.FirstOrDefault(m => m.GetCustomAttribute<FactoryAttribute>() != null);
        if (attributed != null)
        {
            return attributed;
        }

        MethodBase? staticCreate = methods.OfType<MethodInfo>().Where(m => m.Name == "Construct").OrderByDescending(m => m.GetParameters().Length).FirstOrDefault();
        if (staticCreate != null)
        {
            return staticCreate;
        }

        MethodBase? longestConstructor = methods.OfType<ConstructorInfo>().OrderByDescending(m => m.GetParameters().Length).FirstOrDefault();
        if (longestConstructor != null)
        {
            return longestConstructor;
        }

        throw new InvalidOperationException($"No construction method found for service implementation {type.FullName}");
    }

    private Type CloseClass(Type @class, IReadOnlyDictionary<string, Type> typeArguments)
    {
        if (@class.IsGenericTypeDefinition)
        {
            Type[] types = @class.GetGenericArguments();
            for (int i = 0; i < types.Length; i++)
            {
                if (typeArguments.TryGetValue(types[i].Name.ToLower(), out Type? type))
                {
                    types[i] = type;
                }
                else
                {
                    throw new InvalidOperationException($"No type argument given for type parameter {types[i]} in class {@class}");
                }
            }

            return @class.MakeGenericType(types);
        }

        return @class;
    }
}