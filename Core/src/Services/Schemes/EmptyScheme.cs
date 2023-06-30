namespace Markwardt;

public record EmptyScheme : IObjectScheme
{
    public Maybe<IObjectBuilder> GetBuilder(ObjectTag tag)
        => default;

    public Maybe<IObjectBuilder> GetSingletonBuilder(ObjectTag tag)
        => default;
}

public record EmptyScheme<T> : EmptyScheme, IObjectScheme<T>;