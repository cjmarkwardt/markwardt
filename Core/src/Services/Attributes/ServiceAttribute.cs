namespace Markwardt;

public interface IServiceAttribute
{
    public abstract IServiceConfiguration GetConfiguration(Type type);
}

public class ServiceAttribute : Attribute, IServiceAttribute
{
    public ServiceAttribute(Type? implementation = null, OpenServiceKind kind = OpenServiceKind.Natural, Type? arguments = null)
    {
        Implementation = implementation;
        Kind = kind;
        Arguments = arguments;
    }

    public Type? Implementation { get; }
    public OpenServiceKind Kind { get; }
    public Type? Arguments { get; }

    public IServiceConfiguration GetConfiguration(Type type)
        => NaturalConfiguration.Get(type, new NaturalConfigurationOptions() { Implementation = Implementation, Kind = Kind, Arguments = Arguments });
}