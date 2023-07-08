namespace Markwardt;

public class FactoryConfiguration : IServiceConfiguration
{
    public FactoryConfiguration(AsyncFunc<IServiceResolver, IDictionary<string, object?>?, object> factory, ServiceKind kind = ServiceKind.Singleton)
    {
        Builder = new FactoryBuilder(async (resolver, arguments) => await factory(resolver, arguments));
        Kind = kind;
    }

    public IServiceBuilder Builder { get; }
    public ServiceKind Kind { get; }
}