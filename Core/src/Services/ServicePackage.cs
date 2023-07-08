namespace Markwardt;

public interface IServicePackage
{
    void Configure(IServiceContainer container);
}

public class ServicePackage : IServicePackage
{
    public ServicePackage(Action<IServiceContainer> configure)
    {
        this.configure = configure;
    }

    private readonly Action<IServiceContainer> configure;

    public void Configure(IServiceContainer container)
        => configure(container);
}