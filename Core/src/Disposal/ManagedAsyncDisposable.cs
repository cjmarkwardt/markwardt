namespace Markwardt;

public abstract class ManagedAsyncDisposable : ManagedDisposable, IAsyncDisposable
{
    public ManagedAsyncDisposable()
    {
        Disposal.OnSharedDisposal(OnSharedDisposal);
        Disposal.OnAsyncDisposal(OnAsyncDisposal);
    }

    protected new IAsyncDisposalManager Disposal { get; } = new AsyncDisposalManager();

    public async ValueTask DisposeAsync()
        => await Disposal.DisposeAsync();

    protected virtual void OnSharedDisposal() { }

    protected virtual ValueTask OnAsyncDisposal()
        => new ValueTask();
}