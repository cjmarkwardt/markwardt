namespace Markwardt;

public abstract class ServiceExiter : IExiter
{
    protected ServiceExiter(IObjectContainer services)
    {
        this.services = services;
    }

    private readonly IObjectContainer services;

    public bool IsExiting { get; private set; }

    public async void Exit(object? info = null)
    {
        if (IsExiting)
        {
            return;
        }

        IsExiting = true;
        await services.DisposeAsync();
        ExecuteExit(info);
    }

    protected abstract void ExecuteExit(object? info);
}