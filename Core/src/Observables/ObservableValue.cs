namespace Markwardt;

public interface IObservableValue : IObservableTarget
{
    object? Value { get; }

    IObservable<IValueChange> ValueChanges { get; }
}

public interface IObservableValue<out T> : IObservableTarget
{
    IObservableValue Generalized { get; }

    T Value { get; }

    IObservable<IValueChange<T>> ValueChanges { get; }
}