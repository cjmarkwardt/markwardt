namespace Markwardt;

public class DerivedMutableValue<T> : IMutableValue<T>, IMutableValue
{
    public DerivedMutableValue(Func<T> getValue, Action<T> setValue, IObservable<T> changes)
    {
        this.getValue = getValue;
        this.setValue = setValue;
        this.changes = changes;
    }

    private readonly Func<T> getValue;
    private readonly Action<T> setValue;
    private readonly IObservable<T> changes;

    public IMutableValue Generalized => this;

    IValue IValue<T>.Generalized => this;

    public T Get()
        => getValue();

    public void Set(T value)
        => setValue(value);

    public IDisposable Subscribe(IObserver<T> observer)
        => changes.Prepend(getValue()).Subscribe(observer);

    object? IValue.Get()
        => Get();

    void IMutableValue.Set(object? value)
        => Set((T)value!);

    IObservable<object?> IValue.Observe()
        => this.Generalize();
}