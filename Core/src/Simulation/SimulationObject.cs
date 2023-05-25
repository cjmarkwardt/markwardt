namespace Markwardt;

public interface ISimulationObject<TSimulation, TObject, TTransform> : IFullDisposable
    where TSimulation : class, ISimulation<TSimulation, TObject, TTransform>
    where TObject : class, ISimulationObject<TSimulation, TObject, TTransform>
{
    TSimulation Simulation { get; }

    TObject? Parent { get; set; }
    IEnumerable<TObject> Children { get; }
    IEnumerable<ISimulationBehavior> Behaviors { get; }

    TTransform GlobalTransform { get; }
    TTransform LocalTransform { get; }

    void AddBehavior(ISimulationBehavior behavior);
    void RemoveBehavior(ISimulationBehavior behavior);

    TObject CreateChild(string name);

    void Shift(int index);

    void Update();
}

public abstract class SimulationObject<TSimulation, TObject, TTransform> : ManagedAsyncDisposable, ISimulationObject<TSimulation, TObject, TTransform>, IHierarchyController
    where TSimulation : class, ISimulation<TSimulation, TObject, TTransform>
    where TObject : class, ISimulationObject<TSimulation, TObject, TTransform>
{
    public SimulationObject(TSimulation simulation)
    {
        Simulation = simulation;
    }

    private readonly HashSet<ISimulationBehavior> unstartedBehaviors = new();
    private readonly HashSet<ISimulationBehavior> behaviors = new();

    private readonly Sequence<TObject> children = new();

    public TSimulation Simulation { get; }

    private TObject? parent;
    public TObject? Parent
    {
        get => parent;
        set
        {
            if (!parent.NullableEquals(value))
            {
                if (parent == null)
                {

                }
            }
        }
    }

    public IEnumerable<ISimulationBehavior> Behaviors => behaviors;
    public IShiftableSequence<TObject> Children => children;

    public abstract TTransform GlobalTransform { get; }
    public abstract TTransform LocalTransform { get; }

    public void AddBehavior(ISimulationBehavior behavior)
    {
        if (behaviors.Add(behavior))
        {
            unstartedBehaviors.Add(behavior);
            Disposal.Track(behavior);
        }
    }

    public abstract TObject CreateChild(string name);

    public void RemoveBehavior(ISimulationBehavior behavior)
    {
        if (behaviors.Remove(behavior))
        {
            Disposal.ForkDispose(behavior);
        }
    }

    public void Update()
    {
        foreach (ISimulationBehavior behavior in unstartedBehaviors)
        {
            behavior.Start();
        }

        unstartedBehaviors.Clear();

        foreach (ISimulationBehavior behavior in behaviors)
        {
            behavior.Update();
        }
    }

    void IHierarchyParentController.SetParent(object parent)
        => this.parent = (TObject)parent;

    void IHierarchyParentController.ClearParent()
        => parent = null;

    void IHierarchyChildController.AddChild(object child)
        => children.Append((TObject)child);

    void IHierarchyChildController.RemoveChild(object child)
        => children.Remove((TObject)child);

    protected override void OnDisposal()
    {
        base.OnDisposal();

        if (Simulation is IManagedAsyncDisposable managedSimulation && !managedSimulation.IsDisposed)
        {
            using (managedSimulation.Disposal.Stall())
            {
                ExecuteDispose();
            }
        }
        else
        {
            ExecuteDispose();
        }
    }

    protected override async ValueTask OnAsyncDisposal()
    {
        await base.OnAsyncDisposal();

        if (Simulation is IManagedAsyncDisposable managedSimulation && !managedSimulation.IsDisposed)
        {
            using (managedSimulation.Disposal.Stall())
            {
                await ExecuteDisposeAsync();
            }
        }
        else
        {
            await ExecuteDisposeAsync();
        }
    }

    private void ExecuteDispose()
    {
        behaviors.DisposalAll();
        Children.DisposalAll();
    }

    private async ValueTask ExecuteDisposeAsync()
        => await Task.WhenAll(behaviors.DisposeAllAsync(), Children.DisposeAllAsync());
}