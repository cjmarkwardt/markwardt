namespace Markwardt;

public interface IObjectProfile
{
    Type Target { get; }
    IObjectBuilder? Builder { get; }
    IObjectBuilder? SingletonBuilder { get; }
}

public record ObjectProfile(Type Target, IObjectBuilder? Builder, IObjectBuilder? SingletonBuilder) : IObjectProfile
{
    public static ObjectProfile Create(Type target, Type implementation, IArgumentGenerator? arguments, bool isSingleton, IArgumentGenerator? singletonArguments)
        => new ObjectProfile(target, new InstantiationBuilder(target).OverrideArguments(arguments), !isSingleton ? null : new CreatorBuilder(target).OverrideArguments(singletonArguments));

    public static ObjectProfile Create(Type target, Type implementation, IArgumentGenerator? arguments)
    {
        if (target.TryGetCustomAttribute(out SingletonAttribute? singleton))
        {
            return Create(target, implementation, arguments, true, singleton.Arguments == null ? null : new ResolvedArgumentGenerator(singleton.Arguments));
        }
        else
        {
            return Create(target, implementation, arguments, false, null);
        }
    }

    public static ObjectProfile? Scan(Type target)
        => Scan(target, out ObjectProfile? profile) ? profile : null;

    public static bool Scan(Type target, [NotNullWhen(true)] out ObjectProfile? profile)
    {
        Type? implementation = null;
        IArgumentGenerator? arguments = null;

        Type current = target;
        while (true)
        {
            if (!GetImplementation(current, out (Type Implementation, IArgumentGenerator? Arguments)? next))
            {
                if (target.IsInstantiable() && target.TryGetCustomAttribute(out ImplementAttribute? implementAttribute))
                {
                    profile = Create(target, target, implementAttribute.Arguments == null ? null : new ResolvedArgumentGenerator(implementAttribute.Arguments));
                    return true;
                }
                else
                {
                    profile = null;
                    return false;
                }
            }

            implementation = next.Value.Implementation;

            if (next.Value.Arguments != null)
            {
                if (arguments == null)
                {
                    arguments = next.Value.Arguments;
                }
                else
                {
                    arguments = next.Value.Arguments.Stack(arguments);
                }
            }

            if (next.Value.Implementation == current)
            {
                break;
            }
            else
            {
                current = next.Value.Implementation;
            }
        }

        profile = Create(target, implementation, arguments);
        return true;
    }

    private static bool GetImplementation(Type target, [NotNullWhen(true)] out (Type Type, IArgumentGenerator? Arguments)? implementation)
    {
        Type? implementationType = null;
        IArgumentGenerator? arguments = null;

        if (target.TryGetCustomAttribute(out ImplementationAttribute? implementationAttribute))
        {
            implementationType = implementationAttribute.Implementation;

            if (implementationAttribute.Arguments != null)
            {
                arguments = new ResolvedArgumentGenerator(implementationAttribute.Arguments);
            }
        }
        else if (DelegateType.Create(target, out DelegateType? delegateType) && delegateType.Return.TryGetGenericTypeDefinition() == typeof(ValueTask<>))
        {
            implementation = (delegateType.Return.GetGenericArguments().First(), null);
        }
        else if (target.IsInterface && TypeUtils.TryGetType(target.AssemblyQualifiedName.Replace(target.Name, target.Name.Substring(1)), out Type? interfaceImplementation))
        {
            implementation = (interfaceImplementation, null);
        }
        else if (target.IsInstantiable())
        {
            implementation = (target, null);
        }

        if (implementationType == null)
        {
            implementation = null;
            return false;
        }
        else
        {
            implementation = (implementationType, arguments);
            return true;
        }
    }
}