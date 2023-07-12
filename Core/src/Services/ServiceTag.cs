namespace Markwardt;

public interface IServiceTag { }

public record TypeTag(Type Type) : IServiceTag
{
    public static TypeTag Create<T>()
        where T : notnull
        => new TypeTag(typeof(T));
}

public record ConfigurationTag(Type Configuration) : IServiceTag
{
    public static ConfigurationTag Create<TConfiguration>()
        where TConfiguration : IServiceConfiguration, new()
        => new ConfigurationTag(typeof(TConfiguration));
}