namespace Markwardt;

public interface IObjectBuilder
{
    ValueTask<object> Build(IObjectContainer container, IArgumentGenerator? argumentGenerator = null);
}