namespace Markwardt;

public interface IObservableTarget
{
    IObservable<IObservedChange> Changes { get; }
}

public abstract class ObservableTarget : IObservableTarget
{
    private readonly Subject<IObservedChange> changes = new();
    public IObservable<IObservedChange> Changes => changes;

    protected void PushChange(IObservedChange change)
        => changes.OnNext(change);
}