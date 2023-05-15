namespace Markwardt;

public interface IServicePackage
{
    void Configure(IServiceConfiguration services);
}

public static class ServicePackageUtils
{
    public static IServicePackage AsPackage(this Action<IServiceConfiguration> configure)
        => new ServicePackage(configure);
}

public class ServicePackage : IServicePackage
{
    public ServicePackage(Action<IServiceConfiguration> configure)
    {
        this.configure = configure;
    }

    private readonly Action<IServiceConfiguration> configure;

    public void Configure(IServiceConfiguration services)
        => configure(services);
}