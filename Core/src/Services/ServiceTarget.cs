namespace Markwardt;

public interface IServiceTarget
{
    ServiceLifetime Lifetime { get; }
    IServiceBuilder Builder { get; }
}

public record ServiceTarget(ServiceLifetime Lifetime, IServiceBuilder Builder) : IServiceTarget
{
    public static IServiceTarget? FromServiceType(IServiceContainer services, Type service)
    {
        ServiceLifetimeAttribute? attribute = service.GetCustomAttribute<ServiceLifetimeAttribute>();
        IServiceBuilder? builder = ServiceBuilder.FromServiceType(services, service);
        if (builder != null && attribute != null)
        {
            return new ServiceTarget(attribute.Lifetime, builder);
        }

        return null;
    }
}