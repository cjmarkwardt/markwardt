namespace Markwardt;

public interface IWorldObject
{
    delegate IWorldObject Factory(string name);

    string Name { get; set; }

    IWorldRoot Root { get; }
    IWorldObject? Parent { get; set; }
    IEnumerable<IWorldObject> Children { get; }
    IEnumerable<IWorldBehavior> Behaviors { get; }

    Vector3 GlobalPosition { get; set; }
    Vector3 GlobalRotation { get; set; }

    Vector3 LocalPosition { get; set; }
    Vector3 LocalRotation { get; set; }
    Vector3 LocalScale { get; set; }

    void AddBehavior(IWorldBehavior behavior);
    void RemoveBehavior(IWorldBehavior behavior);
}