namespace Markwardt;

public class StoredValue<T> : IMutableValue<T>, IMutableValue
{
    public StoredValue(T value)
    {
        this.value = value;
    }

    private readonly Subject<T> subject = new();

    private T value;

    public IMutableValue Generalized => this;

    IValue IValue<T>.Generalized => this;

    public T Get()
        => value;

    public void Set(T value)
    {
        this.value = value;
        subject.OnNext(value);
    }

    public IDisposable Subscribe(IObserver<T> observer)
        => subject.Prepend(value).Subscribe(observer);

    object? IValue.Get()
        => Get();

    void IMutableValue.Set(object? value)
        => Set((T)value!);

    IObservable<object?> IValue.Observe()
        => this.Generalize();
}