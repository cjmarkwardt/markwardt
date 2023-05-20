namespace Markwardt;

public interface ICollectionChange : IObservedChange
{
    CollectionChangeKind Kind { get; }
    IEnumerable<object?> Items { get; }
}

public interface ICollectionChange<out TItem> : IObservedChange
{
    ICollectionChange Generalized { get; }

    CollectionChangeKind Kind { get; }
    IEnumerable<TItem> Items { get; }
}