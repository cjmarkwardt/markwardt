namespace Markwardt;

public class DefaultSchemeGenerator : IObjectSchemeGenerator
{
    public Maybe<IObjectScheme> Generate(ObjectTag tag)
    {
        if (tag.Scheme != null)
        {
            return ((IObjectScheme)Activator.CreateInstance(tag.Scheme)).AsMaybe();
        }
        else
        {
            return Scan(tag.Type);
        }
    }

    private Maybe<IObjectScheme> Scan(Type type)
    {
        if (type.TryGetCustomAttribute(out ServiceAttribute? serviceAttribute))
        {
            return serviceAttribute.GetScheme(type).AsMaybe();
        }
        else if (DelegateType.Create(type, out DelegateType? delegateType) && delegateType.Return.TryGetGenericTypeDefinition() == typeof(ValueTask<>))
        {
            implementation = (delegateType.Return.GetGenericArguments().First(), null);
        }
        else if (type.IsInterface && type.TryGetInterfaceImplementation(out Type? interfaceImplementation))
        {
            return new ImplementationScheme(interfaceImplementation);
        }
        else if (type.IsInstantiable())
        {
            return new ImplementationScheme(type);
        }
        else
        {
            return default;
        }
    }

    /*public Maybe<IObjectScheme> Scan(Type type)
    {
        Type? implementation = null;
        Maybe<IObjectArgumentGenerator> arguments = default;

        Type current = type;
        while (true)
        {
            if (!GetImplementation(current, out (Type Implementation, IObjectArgumentGenerator? Arguments)? next))
            {
                if (type.IsInstantiable() && type.TryGetCustomAttribute(out ImplementAttribute? implementAttribute))
                {
                    profile = Create(type, type, implementAttribute.Arguments == null ? null : new ResolvedArgumentGenerator(implementAttribute.Arguments));
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



        profile = Create(type, implementation, arguments);
        return true;
    }

    private static bool GetImplementation(Type target, [NotNullWhen(true)] out (Type Type, IObjectArgumentGenerator? Arguments)? implementation)
    {
        Type? implementationType = null;
        Maybe<IObjectArgumentGenerator> arguments = default;

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
    }*/
}