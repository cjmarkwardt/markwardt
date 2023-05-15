namespace Markwardt;

public interface IDisposalManager : IDisposable
{
    bool IsDisposed { get; }

    void Verify();

    void Add(params object?[] targets);
    void Remove(params object?[] targets);

    void OnDisposal(Action action);
}

public class DisposalManager : IDisposalManager
{
    private readonly HashSet<Action> disposalActions = new();
    private readonly HashSet<object> targets = new();

    protected IEnumerable<object> Targets => targets;

    public bool IsDisposed { get; protected set; }

    public void Verify()
    {
        if (IsDisposed)
        {
            throw new ObjectDisposedException(GetType().FullName);
        }
    }

    public void Add(params object?[] targets)
    {
        foreach (object target in targets.WhereNotNull())
        {
            this.targets.Add(target);
        }
    }

    public void Remove(params object?[] targets)
    {
        foreach (object target in targets.WhereNotNull())
        {
            this.targets.Remove(target);
        }
    }

    public void OnDisposal(Action action)
        => disposalActions.Add(action);

    public void Dispose()
    {
        if (!IsDisposed)
        {
            IsDisposed = true;
            PreDispose();
            disposalActions.InvokeAll();
            targets.TryDisposeAll();
        }
    }

    protected virtual void PreDispose() { }
}