namespace Markwardt;

public static class DelegateTypeUtils
{
    public static bool IsDelegate(this Type type)
        => typeof(MulticastDelegate).IsAssignableFrom(type);

    public static DelegateType? AsDelegate(this Type type)
        => DelegateType.Create(type);

    public static bool AsDelegate(this Type type, [NotNullWhen(true)] out DelegateType? delegateType)
        => DelegateType.Create(type, out delegateType);
}

public class DelegateType
{
    public static DelegateType? Create(Type type)
        => type.IsDelegate() ? new DelegateType(type) : null;

    public static bool Create(Type type, [NotNullWhen(true)] out DelegateType? delegateType)
    {
        delegateType = Create(type);
        return delegateType != null;
    }

    private DelegateType(Type type)
    {
        Type = type;

        invoke = new(() => type.GetMethod(nameof(Action.Invoke))!);
        parameters = new(() => invoke.Value.GetParameters());
    }

    private readonly Lazy<MethodInfo> invoke;
    private readonly Lazy<IReadOnlyList<ParameterInfo>> parameters;

    public Type Type { get; }

    public Type Return => invoke.Value.ReturnType;
    public IReadOnlyList<ParameterInfo> Parameters => parameters.Value;

    public delegate object? Implementation(IEnumerable<Argument<object?>> arguments);
    public delegate ValueTask<object?> AsyncImplementation(IEnumerable<Argument<object?>> arguments);

    public DelegateType Close(IEnumerable<Argument<Type>>? typeArguments)
    {
        if (!Type.IsGenericTypeDefinition)
        {
            return this;
        }

        IReadOnlyDictionary<string, Type> typeArgumentLookup = typeArguments?.ToDictionary(a => a.Name, a => a.Value) ?? new Dictionary<string, Type>();
        Type[] types = Type.GetGenericArguments();
        for (int i = 0; i < types.Length; i++)
        {
            if (typeArgumentLookup.TryGetValue(types[i].Name.ToLower(), out Type? type))
            {
                types[i] = type;
            }
            else
            {
                throw new InvalidOperationException($"No type argument given for type parameter {types[i]} in delegate {Type}");
            }
        }

        return new DelegateType(Type.MakeGenericType(types));
    }

    public Delegate Implement(Expression<Implementation> implementation)
    {
        if (Type.IsGenericTypeDefinition)
        {
            throw new InvalidOperationException($"Delegate {Type} must be closed");
        }

        IEnumerable<ParameterExpression> parameters = GenerateParameters();
        return Expression.Lambda(Type, Expression.Convert(Expression.Invoke(implementation, GenerateArguments(parameters)), Return), parameters).Compile();
    }

    public Delegate Implement(Expression<AsyncImplementation> implementation)
    {
        if (Type.IsGenericTypeDefinition)
        {
            throw new InvalidOperationException($"Delegate {Type} must be closed");
        }

        IEnumerable<ParameterExpression> parameters = GenerateParameters();
        return Expression.Lambda(Type, Expression.Call(typeof(TaskUtils), nameof(TaskUtils.Specify), new Type[] { Return.GetGenericArguments().First() }, Expression.Invoke(implementation, GenerateArguments(parameters))), parameters).Compile();
    }

    private IEnumerable<ParameterExpression> GenerateParameters()
        => !Parameters.Any() ? Enumerable.Empty<ParameterExpression>() : Parameters.Select(p => Expression.Parameter(p.ParameterType, p.Name)).ToList();

    private Expression GenerateArguments(IEnumerable<ParameterExpression> parameters)
    {
        ConstructorInfo argumentConstructor = typeof(Argument<object?>).GetConstructor(new Type[] { typeof(string), typeof(object) })!;
        IEnumerable<Expression> arguments = !parameters.Any() ? Enumerable.Empty<Expression>() : parameters.Select(p => Expression.New(argumentConstructor, Expression.Constant(p.Name), Expression.Convert(p, typeof(object))));
        return Expression.Convert(Expression.NewArrayInit(typeof(Argument<object?>), arguments), typeof(IEnumerable<Argument<object?>>));
    }
}