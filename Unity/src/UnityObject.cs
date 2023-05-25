namespace Markwardt;

public class UnityObjectt : IObject3d
{
    public ISimulation3d Simulation => throw new NotImplementedException();

    public IObject3d? Parent { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public IShiftableSequence<IObject3d> Children => throw new NotImplementedException();

    public IEnumerable<ISimulationBehavior> Behaviors => throw new NotImplementedException();

    public ITransform3d GlobalTransform => throw new NotImplementedException();

    public ITransform3d LocalTransform => throw new NotImplementedException();

    public bool IsDisposed => throw new NotImplementedException();

    public void AddBehavior(ISimulationBehavior behavior)
    {
        throw new NotImplementedException();
    }

    public IObject3d CreateChild(string name)
    {
        throw new NotImplementedException();
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }

    public ValueTask DisposeAsync()
    {
        throw new NotImplementedException();
    }

    public void RemoveBehavior(ISimulationBehavior behavior)
    {
        throw new NotImplementedException();
    }

    public void Update()
    {
        throw new NotImplementedException();
    }
}

public class UnityObject : MonoBehaviour, ISimulationObject
{
    public static UnityObject Construct(IWorldRoot root, string name)
    {
        UnityObject obj = new GameObject(name).AddComponent<UnityObject>();
        obj.Root = root;
        return obj;
    }

    public string Name { get => gameObject.name; set => gameObject.name = value; }

    public IWorldRoot Root { get; private set; } = default!;

    public ISimulationObject? Parent
    {
        get => transform.parent == null ? null : transform.parent.gameObject.RequireComponent<UnityObject>();
        set => transform.parent = value == null ? null : ((UnityObject)value).gameObject.transform;
    }

    public IEnumerable<ISimulationObject> Children => transform.Cast<Transform>().Select(x => x.gameObject.RequireComponent<UnityObject>());

    public Vector3 GlobalPosition { get => transform.position.ToGeneralVector(); set => transform.position = value.ToUnityVector(); }
    public Vector3 GlobalRotation { get => transform.rotation.ToGeneralVector(); set => transform.rotation = value.ToQuaternion(); }

    public Vector3 LocalPosition { get => transform.localPosition.ToGeneralVector(); set => transform.localPosition = value.ToUnityVector(); }
    public Vector3 LocalRotation { get => transform.localRotation.ToGeneralVector(); set => transform.localRotation = value.ToQuaternion(); }
    public Vector3 LocalScale { get => transform.localScale.ToGeneralVector(); set => transform.localScale = value.ToUnityVector(); }

    public IEnumerable<ISimulationBehavior> Behaviors => gameObject.GetComponents<UnityBehavior>();

    public void AddBehavior(ISimulationBehavior behavior)
    {
        throw new NotImplementedException();
    }

    public void RemoveBehavior(ISimulationBehavior behavior)
    {
        throw new NotImplementedException();
    }

    private void Start()
    {
        foreach (ISimulationBehavior behavior in Behaviors)
        {
            behavior.Start();
        }
    }

    private void Update()
    {
        foreach (ISimulationBehavior behavior in Behaviors)
        {
            behavior.Update();
        }
    }

    private void OnDestroy()
    {
        foreach (ISimulationBehavior behavior in Behaviors)
        {
            behavior.Destroy();
        }
    }
}