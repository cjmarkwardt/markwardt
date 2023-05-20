namespace Markwardt;

public interface IObservableCollectionSource : IObservableCollection
{
    void Add(IEnumerable<object?> items);
    void Remove(IEnumerable<object?> item);
    void Clear();

}

public interface IObservableCollectionSource<TItem> : IObservableCollection<TItem>, ICollection<TItem>
{
    void Add(IEnumerable<TItem> items);
    void Remove(IEnumerable<TItem> items);
}

public class ObservableCollectionSource<TItem>
{
    
}