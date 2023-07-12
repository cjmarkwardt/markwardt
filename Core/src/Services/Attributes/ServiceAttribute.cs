namespace Markwardt;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Delegate | AttributeTargets.Enum)]
public class ServiceAttribute : BaseServiceAttribute
{
    public ServiceAttribute(Type? implementation = null, OpenServiceKind kind = OpenServiceKind.Natural, Type? arguments = null, string? constructorId = null, string? constructorName = null, Type[]? constructorArguments = null)
    {
        Implementation = implementation;
        Kind = kind;
        Arguments = arguments;
        ConstructorId = constructorId;
        ConstructorName = constructorName;
        ConstructorArguments = constructorArguments;
    }

    public Type? Implementation { get; init; }
    public OpenServiceKind Kind { get; init; }
    public Type? Arguments { get; init; }
    public string? ConstructorId { get; init; }
    public string? ConstructorName { get; init; }
    public Type[]? ConstructorArguments { get; init; }

    public override IServiceConfiguration GetConfiguration(Type type)
        => NaturalConfiguration.Get(type, new NaturalConfigurationOptions()
        {
            Implementation = Implementation,
            Kind = Kind,
            Arguments = Arguments,
            ConstructorName = ConstructorName,
            ConstructorArguments = ConstructorArguments
        });
}