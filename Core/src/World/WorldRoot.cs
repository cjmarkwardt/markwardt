namespace Markwardt;

public interface IWorldRoot
{
    IEnumerable<IWorldObject> Objects { get; }
}