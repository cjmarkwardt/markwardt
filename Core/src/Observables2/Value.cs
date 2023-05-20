namespace Markwardt;

public interface IValue<T>
{
    T Value { get; }

    IObservable<IValueChange<T>> Changes { get; }
}

public interface IMutableValue<T> : IValue<T>
{
    new T Value { get; set; }
}

public static class MutableValueUtils
{
    public static IDisposable Bind<T>(this IMutableValue<T> value, IObservable<T> changes)
        => changes.Subscribe(x => value.Value = x);

    public static IDisposable Bind<T>(this IMutableValue<T> value, IValue<T> target)
        => value.Bind(target.Changes.Select(x => x.NewValue));
}