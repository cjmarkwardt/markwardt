namespace Markwardt;

public interface IObservableCollection : IObservableObject, IEnumerable<object?>, INotifyCollectionChanged
{
    IObservableValue<int> Count { get; }

    IObservable<ICollectionChange> ItemChanges { get; }

    bool Contains(object? item);
}

public interface IObservableCollection<out TItem> : IObservableObject, IReadOnlyCollection<TItem>
{
    IObservableCollection Generalized { get; }

    new IObservableValue<int> Count { get; }

    IObservable<ICollectionChange<TItem>> ItemChanges { get; }

    bool Contains(object? item);
}

public class ObservableCollection
{
    public static IObservableCollectionSource<T> CreateSource<T>()
    {
        throw new NotImplementedException();
    }
}