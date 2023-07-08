namespace Markwardt;

public class NaturalServiceAttribute : Attribute
{
    public NaturalServiceAttribute(Type? implementation = null, OpenServiceKind kind = OpenServiceKind.Natural, Type? arguments = null)
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