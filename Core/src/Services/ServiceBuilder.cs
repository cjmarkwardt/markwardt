namespace Markwardt;

public interface IServiceBuilder
{
    ValueTask<object> Build(IServiceContainer container, IEnumerable<Argument<object?>>? arguments = null, IEnumerable<Argument<Type>>? typeArguments = null);
}

public static class ServiceBuilderUtils
{
    public static IServiceTarget AsTarget(this IServiceBuilder builder, ServiceLifetime lifetime)
        => new ServiceTarget(lifetime, builder);
}

public static class ServiceBuilder
{
    public static IServiceBuilder? FromServiceType(IServiceContainer services, Type service)
    {
        ServiceAttribute? attribute = service.GetCustomAttribute<ServiceAttribute>();
        if (attribute != null)
        {
            return new ClassTargetBuilder(attribute.Implementation)
                .WithMappedArguments(service.GetCustomAttributes<MapArgumentAttribute>().Select(a => a.Mapping))
                .WithMappedTypeArguments(service.GetCustomAttributes<MapTypeArgumentAttribute>().Select(a => a.Mapping))
                .ThroughDelegate(service);
        }

        RoutedServiceAttribute? routedAttribute = service.GetCustomAttribute<RoutedServiceAttribute>();
        if (routedAttribute != null)
        {
            return routedAttribute.Lifetime == ServiceLifetime.Singleton ? services.CreateResolveBuilder(routedAttribute.TargetService) : services.CreateBuilder(routedAttribute.TargetService);
        }

        return null;
    }
}