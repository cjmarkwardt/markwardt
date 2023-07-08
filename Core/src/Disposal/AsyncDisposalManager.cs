namespace Markwardt;

public interface IAsyncDisposalManager : IDisposalManager, ICompositeAsyncDisposable
{
    void OnAsyncDisposal(AsyncAction action);
    void OnSharedDisposal(Action action);
}

public class AsyncDisposalManager : DisposalManager, IAsyncDisposalManager
{
    private readonly HashSet<Action> sharedDisposalActions = new();
    private readonly HashSet<AsyncAction> asyncDisposalActions = new();

    public void OnAsyncDisposal(AsyncAction action)
        => asyncDisposalActions.Add(action);

    public void OnSharedDisposal(Action action)
        => sharedDisposalActions.Add(action);

    public async ValueTask DisposeAsync()
    {
        if (!IsDisposed)
        {
            IsDisposed = true;
            PreDispose();
            await asyncDisposalActions.InvokeAll();
            await Targets.TryDisposeAllAsync();
        }
    }

    protected override void PreDispose()
    {
        base.PreDispose();
        sharedDisposalActions.InvokeAll();
    }
}