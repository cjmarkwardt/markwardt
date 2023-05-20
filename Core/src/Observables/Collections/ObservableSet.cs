namespace Markwardt;

public interface IObservableSet : IObservableCollection { }

public interface IObservableSet<out TItem> : IObservableCollection<TItem>
{
    new IObservableSet Generalized { get; }
}