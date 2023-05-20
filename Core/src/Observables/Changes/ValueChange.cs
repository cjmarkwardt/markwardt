namespace Markwardt;

public interface IValueChange : IObservedChange
{
    object? OldValue { get; }
    object? NewValue { get; }
}

public interface IValueChange<out T> : IObservedChange
{
    IValueChange Generalized { get; }

    T OldValue { get; }
    T NewValue { get; }
}

public record ValueChange<T>(IObservableTarget Target, T OldValue, T NewValue) : IValueChange<T>, IValueChange
{
    public IValueChange Generalized => this;

    object? IValueChange.OldValue => OldValue;
    object? IValueChange.NewValue => NewValue;
}
