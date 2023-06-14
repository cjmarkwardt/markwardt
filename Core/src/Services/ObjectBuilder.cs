namespace Markwardt;

public interface IObjectBuilder
{
    ValueTask<object> Build(IObjectResolver resolver, Maybe<IObjectArgumentGenerator> arguments = default);
}

public interface IObjectBuilder<T> : IObjectBuilder
{
    new ValueTask<T> Build(IObjectResolver resolver, Maybe<IObjectArgumentGenerator> arguments = default);
}