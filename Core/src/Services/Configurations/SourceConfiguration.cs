namespace Markwardt;

public class SourceConfiguration : FactoryConfiguration
{
    public SourceConfiguration(ServiceTag source, AsyncFunc<object, object> getter, ServiceKind kind = ServiceKind.Singleton)
        : base(async (resolver, _) => await getter(await resolver.Resolve(source)), kind) { }
}