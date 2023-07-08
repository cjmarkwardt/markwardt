namespace Markwardt;

public static class NaturalConfiguration
{
    public static IServiceConfiguration? TryGet(Type service, NaturalConfigurationOptions? options = null)
    {
        options = options ?? new NaturalConfigurationOptions();

        if (DelegateType.Create(service, out DelegateType? delegateType) && delegateType.Return.TryGetGenericTypeDefinition() == typeof(ValueTask<>))
        {
            Type result = delegateType.Return.GetGenericArguments().First();
            if (options.Implementation != null && options.Implementation.IsInstantiable() && result.IsAssignableFrom(options.Implementation))
            {
                return new InstantiationBuilder(options.Implementation).OverrideArguments(options.Arguments).ThroughDelegate(service).AsConfiguration(options.Kind.Close(ServiceKind.Singleton));
            }
            else if (result.IsInterface && result.TryGetInterfaceImplementation(out Type? interfaceImplementation))
            {
                return new InstantiationBuilder(interfaceImplementation).OverrideArguments(options.Arguments).ThroughDelegate(service).AsConfiguration(options.Kind.Close(ServiceKind.Singleton));
            }
            else if (result.IsInstantiable())
            {
                return new InstantiationBuilder(result).OverrideArguments(options.Arguments).ThroughDelegate(service).AsConfiguration(options.Kind.Close(ServiceKind.Singleton));
            }
        }

        if (service.IsInterface)
        {
            if (options.Implementation != null && options.Implementation.IsInstantiable() && service.IsAssignableFrom(options.Implementation))
            {
                return new InstantiationBuilder(options.Implementation).OverrideArguments(options.Arguments).AsConfiguration(options.Kind.Close(ServiceKind.Transient));
            }
            else if (service.TryGetInterfaceImplementation(out Type? interfaceImplementation))
            {
                return new InstantiationBuilder(interfaceImplementation).OverrideArguments(options.Arguments).AsConfiguration(options.Kind.Close(ServiceKind.Transient));
            }
        }

        if (options.Implementation != null && options.Implementation.IsInstantiable() && service.IsAssignableFrom(options.Implementation))
        {
            return new InstantiationBuilder(options.Implementation).OverrideArguments(options.Arguments).AsConfiguration(options.Kind.Close(ServiceKind.Transient));
        }

        if (service.IsInstantiable())
        {
            return new InstantiationBuilder(service).OverrideArguments(options.Arguments).AsConfiguration(options.Kind.Close(ServiceKind.Transient));
        }

        return null;
    }

    public static bool TryGet(Type service, [NotNullWhen(true)] out IServiceConfiguration? configuration, NaturalConfigurationOptions? options = null)
    {
        configuration = TryGet(service, options);
        return configuration != null;
    }

    public static IServiceConfiguration Get(Type service, NaturalConfigurationOptions? options = null)
        => TryGet(service, options).NotNull($"Type {service} has no natural configuration");
}