namespace Markwardt;

public interface IWorldBehavior : IMultiDisposable
{
    IWorldObject Object { get; }

    void Initialize(IWorldObject obj);

    void Start();
    void Update();
}

public abstract class WorldBehavior : ManagedAsyncDisposable, IWorldBehavior
{
    private IWorldObject? obj;
    public IWorldObject Object => obj ?? throw new InvalidOperationException();

    public void Initialize(IWorldObject obj)
    {
        if (obj != null)
        {
            throw new InvalidOperationException("Behavior already initialized");
        }

        this.obj = obj;
    }

    public virtual void Start() { }
    public virtual void Update() { }
    public virtual void Destroy() { }
}