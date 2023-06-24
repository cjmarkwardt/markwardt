namespace Markwardt;

public static class ObservableUtils
{
    public static IObservable<object?> Generalize<T>(this IObservable<T> observable)
        => observable.Select(x => (object?)x);
}