namespace Markwardt;

public interface IAsyncDisposalManager : IDisposalManager, IFullDisposable
{
    IDisposable Stall();

    void AddAsyncHandler(AsyncAction action);
    void AddSharedHandler(Action action);

    IDisposable Fork(Func<CancellationToken, ValueTask> action);
}

public class AsyncDisposalManager : DisposalManager, IAsyncDisposalManager
{
    private readonly HashSet<TaskCompletionSource<bool>> stalls = new();
    private readonly HashSet<Action> sharedHandlers = new();
    private readonly HashSet<AsyncAction> asyncHandlers = new();

    public IDisposable Stall()
    {
        TaskCompletionSource<bool> completion = new();
        stalls.Add(completion);
        return new Disposable(() =>
        {
            stalls.Remove(completion);
            completion.SetResult(true);
        });
    }

    public void AddAsyncHandler(AsyncAction action)
        => asyncHandlers.Add(action);

    public void AddSharedHandler(Action action)
        => sharedHandlers.Add(action);

    public IDisposable Fork(Func<CancellationToken, ValueTask> action)
    {
        IDisposable stall = Stall();
        CancellationTokenSource cancellation = CancellationTokenSource.CreateLinkedTokenSource(ExecutionCancellation);
        TaskUtils.Fork(async () =>
        {
            await action(ExecutionCancellation);
            cancellation.Dispose();
            stall.Dispose();
        });
        
        return new Disposable(() => cancellation.Cancel());
    }

    public override void ForkDispose(params object?[] targets)
    {
        Untrack(targets);

        foreach (object? target in targets)
        {
            if (target is IAsyncDisposable asyncDisposable)
            {
                Fork(async _ => await asyncDisposable.DisposeAsync());
            }
            else
            {
                target.TryDispose();
            }
        }

        base.ForkDispose(targets);
    }

    public async ValueTask DisposeAsync()
    {
        if (!IsDisposed)
        {
            IsDisposed = true;
            CancelExecution();
            await stalls.Select(x => x.Task).WhenAll();
            PreDispose();
            await asyncHandlers.InvokeAll();
            await Targets.TryDisposeAllAsync();
        }
    }

    protected override void PreDispose()
    {
        base.PreDispose();
        sharedHandlers.InvokeAll();
    }
}