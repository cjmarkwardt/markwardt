namespace Markwardt;

public interface ISimulation<TSimulation, TObject, TTransform> : IFullDisposable
    where TSimulation : class, ISimulation<TSimulation, TObject, TTransform>
    where TObject : class, ISimulationObject<TSimulation, TObject, TTransform>
{
    IEnumerable<TObject> Objects { get; }

    TObject Create(string name);
    void Destroy(TObject obj);
}

public interface IHierarchyChildController
{
    void AddChild(object child);
    void RemoveChild(object child);
}

public interface IHierarchyParentController
{
    object? Parent { get; }
    
    void SetParent(object parent);
    void ClearParent();
}

public interface IHierarchyController : IHierarchyParentController, IHierarchyChildController { }

public static class HierarchyUtils
{
    public static void SetParent(object target, object parent)
    {
        if (target is IHierarchyParentController controller)
        {
            if (controller.Parent != null && controller.Parent is IHierarchyChildController oldParentController)
            {
                oldParentController.RemoveChild(target);
            }

            if (parent is IHierarchyChildController newParentController)
            {
                newParentController.AddChild(target);
            }

            controller.SetParent(parent);
        }
    }

    public static void ClearParent(object target)
    {
        if (target is IHierarchyParentController controller)
        {
            if (controller.Parent != null && controller.Parent is IHierarchyChildController oldParentController)
            {
                oldParentController.RemoveChild(target);
            }

            controller.ClearParent();
        }
    }

    public static void AddChild(object target, object child)
        => SetParent(child, target);

    public static void RemoveChild(object target, object child)
        => ClearParent(child);
}

public abstract class Simulation<TSimulation, TObject, TTransform> : ManagedAsyncDisposable, ISimulation<TSimulation, TObject, TTransform>, IHierarchyChildController
    where TSimulation : class, ISimulation<TSimulation, TObject, TTransform>
    where TObject : class, ISimulationObject<TSimulation, TObject, TTransform>
{
    private readonly Sequence<TObject> objects = new();
    public IShiftableSequence<TObject> Objects => objects;

    public TObject Create(string name)
    {
        TObject obj = CreateObject(name);
        objects.Append(obj);
        return obj;
    }

    public void Update()
    {
        foreach (TObject obj in RecursiveUtils.Chain(Objects, true, x => x.Children))
        {
            obj.Update();
        }
    }

    public void Destroy(TObject obj)
    {
        if (objects.Remove(obj))
        {
            Disposal.ForkDispose(obj);
        }
    }

    protected abstract TObject CreateObject(string name);

    void IHierarchyChildController.AddChild(object obj)
        => objects.Append((TObject)obj);

    void IHierarchyChildController.RemoveChild(object obj)
        => objects.Remove((TObject)obj);

    protected override void OnDisposal()
    {
        base.OnDisposal();
        Objects.DisposalAll();
    }

    protected override async ValueTask OnAsyncDisposal()
    {
        await base.OnAsyncDisposal();
        await Objects.DisposeAllAsync();
    }
}