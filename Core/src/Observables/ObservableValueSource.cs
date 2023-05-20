namespace Markwardt;

public interface IObservableValueSource : IObservableValue
{
    new object? Value { get; set; }
}

public interface IObservableValueSource<T> : IObservableValue<T>
{
    new IObservableValueSource Generalized { get; }

    new T Value { get; set; }
}

public class ObservableValueSource<T> : ObservableTarget, IObservableValueSource<T>, IObservableValueSource
{
    public ObservableValueSource(T value)
    {
        Value = value;
    }

    public IObservableValueSource Generalized => this;

    public T Value { get; set; }

    public IObservable<IValueChange<T>> ValueChanges => Changes.OfType<IValueChange<T>>();

    IObservableValue IObservableValue<T>.Generalized => this;

    T IObservableValue<T>.Value => Value;

    object? IObservableValueSource.Value { get => Value; set => Value = (T)value!; }

    object? IObservableValue.Value => Value;

    IObservable<IValueChange> IObservableValue.ValueChanges => ValueChanges.Select(x => x.Generalized);
}