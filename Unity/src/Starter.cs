namespace Markwardt;

public class Starter<T> : MonoBehaviour
    where T : notnull
{
    private async void Start()
    {
        ServiceContainer container = new();
        Configure(container);
        await container.Resolve<T>();
    }

    protected virtual void Configure(IServiceContainer container) { }
}