namespace Markwardt;

public record ImplementationScheme<T> : IObjectScheme<T>
{
    public IObjectArgumentGenerator? Arguments { get; init; }

    public bool IsSingleton { get; init; }
    public IObjectArgumentGenerator? SingletonArguments { get; init; }

    public Maybe<IObjectBuilder> GetBuilder(ObjectTag tag)
        => new InstantiationBuilder(typeof(T));

    public Maybe<IObjectBuilder> GetSingletonBuilder(ObjectTag tag)
        => Maybe.If<IObjectBuilder>(IsSingleton, () => new CreationBuilder(tag));
}