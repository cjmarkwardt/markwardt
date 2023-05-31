namespace Markwardt;

public interface IMethodLocator
{
    MethodBase? Locate(Type type);
}