namespace Markwardt;

public class MemoryStore : IEntityStore
{
    private readonly Dictionary<string, IIdentifiable> objects = new();

    public ValueTask Delete(IEnumerable<string> ids)
    {
        foreach (string id in ids)
        {
            objects.Remove(id);
        }

        return default;
    }

    public ValueTask<IEnumerable<IIdentifiable>> Load(IEnumerable<string> ids)
    {
        List<IIdentifiable> objects = new();
        foreach (string id in ids)
        {
            objects.Add(this.objects[id]);
        }

        return new ValueTask<IEnumerable<IIdentifiable>>(objects);
    }

    public ValueTask Save(IEnumerable<IIdentifiable> targets)
        => default;
}