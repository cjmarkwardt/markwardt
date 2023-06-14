namespace Markwardt;

public record EmptyScheme<T> : IObjectScheme<T>
{
    public Maybe<IObjectBuilder> GetBuilder(ObjectTag tag)
        => default;

    public Maybe<IObjectBuilder> GetSingletonBuilder(ObjectTag tag)
        => default;
}