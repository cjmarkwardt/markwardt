namespace Markwardt;

public class MethodTargetBuilder : IServiceBuilder
{
    public MethodTargetBuilder(MethodBase method)
    {
        this.method = method;
    }

    private readonly MethodBase method;
    private readonly Dictionary<IValueDictionary<string, Type>, Target> targets = new();

    public async ValueTask<object> Build(IServiceContainer container, IEnumerable<Argument<object?>>? arguments = null, IEnumerable<Argument<Type>>? typeArguments = null)
    {
        IValueDictionary<string, Type> typeArgumentsValue = typeArguments.ToValueDictionary(a => a.Name, a => a.Value);
        MethodBase method = CloseMethod(typeArgumentsValue);

        if (!targets.TryGetValue(typeArgumentsValue, out Target? target))
        {
            target = GenerateTarget(method, typeArgumentsValue);
            targets.Add(typeArgumentsValue, target);
        }

        return await target.Invoke(await ResolveArguments(container, method, arguments));
    }

    private MethodBase CloseMethod(IReadOnlyDictionary<string, Type> typeArguments)
    {
        if (method is MethodInfo directMethod && directMethod.IsGenericMethodDefinition)
        {
            Type[] types = directMethod.GetGenericArguments();
            for (int i = 0; i < types.Length; i++)
            {
                if (typeArguments.TryGetValue(types[i].Name.ToLower(), out Type? type))
                {
                    types[i] = type;
                }
                else
                {
                    throw new InvalidOperationException($"No type argument given for type parameter {types[i]} in method {method}");
                }
            }

            return directMethod.MakeGenericMethod(types);
        }

        return method;
    }
    
    private async ValueTask<object?[]?> ResolveArguments(IServiceContainer container, MethodBase method, IEnumerable<Argument<object?>>? arguments)
    {
        IReadOnlyList<ParameterInfo> parameters = method.GetParameters();
        if (!parameters.Any())
        {
            return null;
        }

        IReadOnlyDictionary<string, object?>? argumentLookup = arguments?.ToDictionary(a => a.Name, a => a.Value);
        object?[] resolvedArguments = new object?[parameters.Count];
        for (int i = 0; i < parameters.Count; i++)
        {
            ParameterInfo parameter = parameters[i];
            object? value;
            if (argumentLookup != null && argumentLookup.TryGetValue(parameter.Name!.ToLower(), out object? argument))
            {
                value = argument;
            }
            else
            {
                object? instance = await container.TryResolve(parameter.ParameterType);
                if (instance != null)
                {
                    value = instance;
                }
                else if (parameter.HasDefaultValue)
                {
                    value = parameter.DefaultValue;
                }
                else
                {
                    throw new InvalidOperationException($"Unable to resolve type {parameter.ParameterType} for parameter {parameter.Name}");
                }
            }

            resolvedArguments[i] = value;
        }

        return resolvedArguments;
    }

    private Target GenerateTarget(MethodBase method, IReadOnlyDictionary<string, Type> typeArguments)
    {
        ParameterExpression injectedArguments = Expression.Parameter(typeof(object?[]));
        
        IEnumerable<Expression> CreateArguments(MethodBase method)
            => method.GetParameters().Select((p, i) => Expression.Convert(Expression.ArrayIndex(injectedArguments, Expression.Constant(i)), p.ParameterType));

        Expression body;
        if (method is ConstructorInfo constructor)
        {
            body = Expression.New(typeof(ValueTask<object>).GetConstructor(new Type[] { typeof(object) })!, Expression.New(constructor, CreateArguments(constructor)));
        }
        else if (method is MethodInfo directMethod)
        {
            Type returnType = directMethod.ReturnType;

            if (!directMethod.IsStatic)
            {
                throw new InvalidOperationException($"Method {method} must be static");
            }
            else if (returnType == typeof(void) || returnType == typeof(Task) || returnType == typeof(ValueTask))
            {
                throw new InvalidOperationException($"Method {method} must return a result");
            }

            if (returnType.TryGetGenericTypeDefinition() == typeof(ValueTask<>))
            {
                body = Expression.Call(typeof(TaskUtils), nameof(TaskUtils.Generalize), new Type[] { directMethod.ReturnType.GetGenericArguments().First() }, Expression.Call(directMethod, CreateArguments(directMethod)));
            }
            else if (returnType.TryGetGenericTypeDefinition() == typeof(Task<>))
            {
                throw new NotImplementedException($"Method {method} must return a ValueTask instead of a Task");
            }
            else
            {
                body = Expression.New(typeof(ValueTask<object>).GetConstructor(new Type[] { typeof(object) })!, Expression.Call(directMethod, CreateArguments(directMethod)));
            }
        }
        else
        {
            throw new InvalidOperationException();
        }

        return Expression.Lambda<Target>(body, injectedArguments).Compile();
    }

    private delegate ValueTask<object> Target(object?[]? arguments);
}