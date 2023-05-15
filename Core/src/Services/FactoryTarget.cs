namespace Markwardt;

public class FactoryTarget : IServiceTarget
{
    public FactoryTarget(IServiceCreator creator, Type service)
    {
        Builder = creator.CreateBuilder(service).ThroughDelegate(typeof(Factory<>).MakeGenericType(service));
    }

    public ServiceLifetime Lifetime => ServiceLifetime.Singleton;
    
    public IServiceBuilder Builder { get; }
}