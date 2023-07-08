namespace Markwardt;

public class FactoryBuilder : IServiceBuilder
{
    public FactoryBuilder(AsyncFunc<IServiceResolver, IDictionary<string, object?>?, object> factory)
    {
        this.factory = factory;
    }

    private readonly AsyncFunc<IServiceResolver, IDictionary<string, object?>?, object> factory;

    public async ValueTask<object> Build(IServiceResolver resolver, IServiceArgumentGenerator? arguments = null)
        => await factory(resolver, await arguments.Generate(resolver));
}
