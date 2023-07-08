namespace Markwardt;

public interface IServiceConfiguration
{
    IServiceBuilder Builder { get; }
    ServiceKind Kind { get; }
}

public static class ServiceConfigurationUtils
{
    public static IServiceConfiguration AsConfiguration(this IServiceBuilder builder, ServiceKind kind)
        => new ServiceConfiguration(builder, kind);

    public static IServiceConfiguration AsTransient(this IServiceBuilder builder)
        => builder.AsConfiguration(ServiceKind.Transient);

    public static IServiceConfiguration AsSingleton(this IServiceBuilder builder)
        => builder.AsConfiguration(ServiceKind.Singleton);
}

public record ServiceConfiguration(IServiceBuilder Builder, ServiceKind Kind) : IServiceConfiguration;