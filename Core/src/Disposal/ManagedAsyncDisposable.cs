namespace Markwardt;

public interface IManagedAsyncDisposable : IFullDisposable
{
    IAsyncDisposalManager Disposal { get; }
}

public abstract class ManagedAsyncDisposable : IManagedAsyncDisposable
{
    public ManagedAsyncDisposable()
    {
        Disposal.AddHandler(OnDisposal);
        Disposal.AddSharedHandler(OnSharedDisposal);
        Disposal.AddAsyncHandler(OnAsyncDisposal);
    }

    public bool IsDisposed => Disposal.IsDisposed;

    public IAsyncDisposalManager Disposal { get; } = new AsyncDisposalManager();

    public void Dispose()
        => Disposal.Dispose();

    public async ValueTask DisposeAsync()
        => await Disposal.DisposeAsync();

    protected virtual void OnDisposal() { }

    protected virtual void OnSharedDisposal() { }

    protected virtual ValueTask OnAsyncDisposal()
        => new ValueTask();
}