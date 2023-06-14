namespace Markwardt;

public interface IObjectScheme
{
    Maybe<IObjectBuilder> GetBuilder(ObjectTag tag);
    Maybe<IObjectBuilder> GetSingletonBuilder(ObjectTag tag);
}

public interface IObjectScheme<T> : IObjectScheme { }