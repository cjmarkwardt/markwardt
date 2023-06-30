namespace Markwardt;

public record InstanceScheme(object GeneralInstance) : IObjectScheme
{
    public Maybe<IObjectBuilder> GetBuilder(ObjectTag tag)
        => default;

    public Maybe<IObjectBuilder> GetSingletonBuilder(ObjectTag tag)
        => new FixedBuilder(GeneralInstance);
}

public record InstanceScheme<T>(T Instance) : InstanceScheme(Instance), IObjectScheme<T>
    where T : notnull;