namespace Markwardt;



public class UnityObject : MonoBehaviour, IWorldObject
{
    public static UnityObject Construct(IWorldRoot root, string name)
    {
        UnityObject obj = new GameObject(name).AddComponent<UnityObject>();
        obj.Root = root;
        return obj;
    }

    public string Name { get => gameObject.name; set => gameObject.name = value; }

    public IWorldRoot Root { get; private set; } = default!;

    public IWorldObject? Parent
    {
        get => transform.parent == null ? null : transform.parent.gameObject.RequireComponent<UnityObject>();
        set => transform.parent = value == null ? null : ((UnityObject)value).gameObject.transform;
    }

    public IEnumerable<IWorldObject> Children => transform.Cast<Transform>().Select(x => x.gameObject.RequireComponent<UnityObject>());

    public Vector3 GlobalPosition { get => transform.position.ToGeneralVector(); set => transform.position = value.ToUnityVector(); }
    public Vector3 GlobalRotation { get => transform.rotation.ToGeneralVector(); set => transform.rotation = value.ToQuaternion(); }

    public Vector3 LocalPosition { get => transform.localPosition.ToGeneralVector(); set => transform.localPosition = value.ToUnityVector(); }
    public Vector3 LocalRotation { get => transform.localRotation.ToGeneralVector(); set => transform.localRotation = value.ToQuaternion(); }
    public Vector3 LocalScale { get => transform.localScale.ToGeneralVector(); set => transform.localScale = value.ToUnityVector(); }

    public IEnumerable<IWorldBehavior> Behaviors => gameObject.GetComponents<UnityBehavior>();

    public void AddBehavior(IWorldBehavior behavior)
    {
        throw new NotImplementedException();
    }

    public void RemoveBehavior(IWorldBehavior behavior)
    {
        throw new NotImplementedException();
    }

    private void Start()
    {
        foreach (IWorldBehavior behavior in Behaviors)
        {
            behavior.Start();
        }
    }

    private void Update()
    {
        foreach (IWorldBehavior behavior in Behaviors)
        {
            behavior.Update();
        }
    }

    private void OnDestroy()
    {
        foreach (IWorldBehavior behavior in Behaviors)
        {
            behavior.Destroy();
        }
    }
}