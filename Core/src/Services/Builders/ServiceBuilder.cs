namespace Markwardt;

public interface IServiceBuilder
{
    ValueTask<object> Build(IServiceResolver resolver, IServiceArgumentGenerator? arguments = null);
}