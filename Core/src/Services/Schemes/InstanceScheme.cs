namespace Markwardt;

public record InstanceScheme<T>(T instance) : IObjectScheme<T>
{
    public Maybe<IObjectBuilder> GetBuilder(ObjectTag tag)
        => default;

    public Maybe<IObjectBuilder> GetSingletonBuilder(ObjectTag tag)
        => new FixedBuilder(instance);
}