namespace Markwardt;

public record ImplementationScheme(Type Type) : IObjectScheme
{
    public IObjectArgumentGenerator? Arguments { get; init; }

    public bool IsSingleton { get; init; }
    public IObjectArgumentGenerator? SingletonArguments { get; init; }

    public Maybe<IObjectBuilder> GetBuilder(ObjectTag tag)
        => new InstantiationBuilder(Type);

    public Maybe<IObjectBuilder> GetSingletonBuilder(ObjectTag tag)
        => Maybe.If<IObjectBuilder>(IsSingleton, () => new CreationBuilder(tag));
}

public record ImplementationScheme<T>() : ImplementationScheme(typeof(T)), IObjectScheme<T>;