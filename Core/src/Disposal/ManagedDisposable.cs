namespace Markwardt;

public abstract class ManagedDisposable : IDisposable
{
    public ManagedDisposable()
    {
        Disposal.OnDisposal(OnDisposal);
    }

    protected IDisposalManager Disposal { get; } = new DisposalManager();

    public void Dispose()
        => Disposal.Dispose();

    protected virtual void OnDisposal() { }
}