namespace Markwardt;

public interface IObjectBuilder
{
    ValueTask<object> Build(IObjectContainer container, Maybe<IObjectArgumentGenerator> arguments = default);
}

public interface IObjectBuilder<T> : IObjectBuilder
{
    new ValueTask<T> Build(IObjectContainer container, Maybe<IObjectArgumentGenerator> arguments = default);
}