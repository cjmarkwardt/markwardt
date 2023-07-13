namespace Markwardt;

public interface IEntity : ICompositeDisposable, IIdentifiable { }

public abstract class Entity : ManagedDisposable, IEntity
{
    public Entity(string id)
    {
        Id = id;
    }

    public string Id { get; }

    void ICompositeDisposable.Add(params object?[] targets)
        => Disposal.Add(targets);

    void ICompositeDisposable.Remove(params object?[] targets)
        => Disposal.Remove(targets);
}