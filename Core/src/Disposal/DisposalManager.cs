namespace Markwardt;

public interface IDisposalManager : ITrackedDisposable
{
    CancellationToken ExecutionCancellation { get; }

    void Verify();

    void Track(params object?[] targets);
    void Untrack(params object?[] targets);

    void AddHandler(Action action);

    void ForkDispose(params object?[] targets);
}

public class DisposalManager : IDisposalManager
{
    private readonly CancellationTokenSource executionCancellation = new();
    private readonly HashSet<Action> handlers = new();
    private readonly HashSet<object> targets = new();

    protected IEnumerable<object> Targets => targets;

    public bool IsDisposed { get; protected set; }

    public CancellationToken ExecutionCancellation => executionCancellation.Token;

    public void Verify()
    {
        if (IsDisposed)
        {
            throw new ObjectDisposedException(GetType().FullName);
        }
    }

    public void Track(params object?[] targets)
    {
        foreach (object target in targets.WhereNotNull())
        {
            this.targets.Add(target);
        }
    }

    public void Untrack(params object?[] targets)
    {
        foreach (object target in targets.WhereNotNull())
        {
            this.targets.Remove(target);
        }
    }

    public void AddHandler(Action action)
        => handlers.Add(action);

    public virtual void ForkDispose(params object?[] targets)
    {
        Untrack(targets);

        foreach (object? target in targets)
        {
            target.TryDispose();
        }
    }

    public void Dispose()
    {
        if (!IsDisposed)
        {
            IsDisposed = true;
            CancelExecution();
            PreDispose();
            handlers.InvokeAll();
            targets.TryDisposeAll();
        }
    }

    protected void CancelExecution()
    {
        executionCancellation.Cancel();
        executionCancellation.Dispose();
    }

    protected virtual void PreDispose()
    {
        executionCancellation.Cancel();
        executionCancellation.Dispose();
    }
}