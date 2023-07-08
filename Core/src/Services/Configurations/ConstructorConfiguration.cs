namespace Markwardt;

public class ConstructorConfiguration : IServiceConfiguration
{
    public ConstructorConfiguration(Type implementation, ServiceKind kind = ServiceKind.Singleton, IDictionary<string, object?>? arguments = null)
    {
        Builder = new InstantiationBuilder(implementation).OverrideArguments(arguments);
        Kind = kind;
    }

    public IServiceBuilder Builder { get; }
    public ServiceKind Kind { get; }
}