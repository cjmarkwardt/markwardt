namespace Markwardt;

public record ServiceTag(Type Type, Type? Configuration = null)
{
    public static ServiceTag Create<T>()
        where T : notnull
        => new ServiceTag(typeof(T));

    public static ServiceTag Create<T, TConfiguration>()
        where T : notnull
        where TConfiguration : IServiceConfiguration, new()
        => new ServiceTag(typeof(T), typeof(TConfiguration));
}