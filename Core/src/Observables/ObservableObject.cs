namespace Markwardt;

public interface IObservableObject : IObservableTarget, INotifyPropertyChanged
{
    IReadOnlyDictionary<string, IObservableTarget> Members { get; }

    IObservable<IObjectChange> MemberChanges { get; }
}

public abstract class ObservableObject : ObservableTarget, IObservableObject
{
    private readonly Dictionary<string, IObservableTarget> members = new();
    public IReadOnlyDictionary<string, IObservableTarget> Members => members;

    public IObservable<IObjectChange> MemberChanges => Changes.OfType<IObjectChange>();

    public event PropertyChangedEventHandler? PropertyChanged;

    protected IDisposable AddMember(string name, IObservableTarget target)
    {
        members.Add(name, target);
        return target.Changes.Subscribe(change =>
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
            PushChange(new ObjectChange(this, target, change));
        });
    }
}