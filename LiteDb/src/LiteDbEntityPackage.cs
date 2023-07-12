namespace Markwardt;

public class LiteDbEntityPackage : IServicePackage
{
    public void Configure(IServiceContainer container)
    {
        container.ConfigureDelegateConstructor<IEntityStore.FileFactory, LiteDbStore>();
    }
}