namespace Markwardt;

public class DerivedValue<T> : IValue<T>, IValue
{
    public DerivedValue(Func<T> getValue, IObservable<T> changes)
    {
        this.getValue = getValue;
        this.changes = changes;
    }

    private readonly Func<T> getValue;
    private readonly IObservable<T> changes;

    public IValue Generalized => this;

    public T Get()
        => getValue();

    public IDisposable Subscribe(IObserver<T> observer)
        => changes.Prepend(getValue()).Subscribe(observer);

    object? IValue.Get()
        => Get();

    IObservable<object?> IValue.Observe()
        => this.Generalize();
}