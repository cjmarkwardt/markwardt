namespace Markwardt;

public class MethodInvoker
{
    private delegate ValueTask<object> InvokeTarget(object?[]? arguments);
    
    public MethodInvoker(MethodBase method)
    {
        this.method = method;
        target = GenerateTarget();
    }

    private readonly MethodBase method;
    private readonly InvokeTarget target;

    public async ValueTask<object> Invoke(IObjectContainer container, IDictionary<string, object?>? arguments)
        => await target.Invoke(await ResolveArguments(container, arguments));

    private InvokeTarget GenerateTarget()
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

        return Expression.Lambda<InvokeTarget>(body, injectedArguments).Compile();
    }

    private async ValueTask<object?[]?> ResolveArguments(IObjectContainer container, IDictionary<string, object?>? arguments)
    {
        IReadOnlyList<ParameterInfo> parameters = method.GetParameters();
        if (!parameters.Any())
        {
            return null;
        }

        object?[] resolvedArguments = new object?[parameters.Count];
        for (int i = 0; i < parameters.Count; i++)
        {
            ParameterInfo parameter = parameters[i];
            object? value;
            if (arguments != null && arguments.TryGetValue(parameter.Name!.ToLower(), out object? argument))
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
}