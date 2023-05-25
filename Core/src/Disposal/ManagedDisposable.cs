namespace Markwardt;

public interface IManagedDisposable : ITrackedDisposable
{
    IDisposalManager Disposal { get; }
}

public abstract class ManagedDisposable : IManagedDisposable
{
    public ManagedDisposable()
    {
        Disposal.AddHandler(OnDisposal);
    }

    public bool IsDisposed => Disposal.IsDisposed;

    public IDisposalManager Disposal { get; } = new DisposalManager();

    public void Dispose()
        => Disposal.Dispose();

    protected virtual void OnDisposal() { }
}