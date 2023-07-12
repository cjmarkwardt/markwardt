namespace Markwardt;

public interface IEntitySystem
{
    Claim<T> Create<T>(string? name, Func<string, T> create)
        where T : notnull, IIdentifiable;

    ValueTask<IEnumerable<Claim<T>>> Load<T>(IEnumerable<string> ids)
        where T : notnull, IIdentifiable;

    ValueTask<Claim<T>> LoadNamed<T>(string name)
        where T : notnull, IIdentifiable;

    ValueTask Delete(IEnumerable<string> ids);

    void Mark(IEnumerable<string> ids);

    ValueTask Save();
}

public static class EntitySystemUtils
{
    public static Claim<T> Create<T>(this IEntitySystem system, Func<string, T> create)
        where T : notnull, IIdentifiable
        => system.Create(null, create);

    public static async ValueTask<IEnumerable<Claim<T>>> Load<T>(this IEntitySystem system, IEnumerable<Id<T>> ids)
        where T : notnull, IIdentifiable
        => await system.Load<T>(ids.Select(x => x.Value));

    public static async ValueTask<Claim<T>> Load<T>(this IEntitySystem system, string id)
        where T : notnull, IIdentifiable
        => (await system.Load<T>(Enumerable.Repeat(id, 1))).First();

    public static async ValueTask<Claim<T>> Load<T>(this IEntitySystem system, Id<T> id)
        where T : notnull, IIdentifiable
        => await system.Load<T>(id.Value);

    public static async ValueTask Delete(this IEntitySystem system, string id)
        => await system.Delete(Enumerable.Repeat(id, 1));

    public static void Mark(this IEntitySystem system, string id)
        => system.Mark(Enumerable.Repeat(id, 1));
}

public class EntitySystem : IEntitySystem
{
    public EntitySystem(IIdGenerator idGenerator, IEntityStore store)
    {
        this.idGenerator = idGenerator;
        this.store = store;
    }

    private readonly IIdGenerator idGenerator;
    private readonly IEntityStore store;
    private readonly Dictionary<string, Entry> entries = new();

    public Claim<T> Create<T>(string? name, Func<string, T> create)
        where T : notnull, IIdentifiable
        => CreateEntry<T>(create(CreateId(name)), true);

    public async ValueTask<IEnumerable<Claim<T>>> Load<T>(IEnumerable<string> ids)
        where T : notnull, IIdentifiable
    {
        List<Claim<T>> claims = new();
        List<string> toLoad = ids.ToList();

        foreach (string id in ids)
        {
            if (entries.TryGetValue(id, out Entry? entry))
            {
                claims.Add(entry.Claim<T>());
            }
        }

        foreach (IIdentifiable obj in await store.Load(toLoad))
        {
            if (entries.TryGetValue(obj.Id, out Entry? entry))
            {
                claims.Add(entry.Claim<T>());
            }
            else
            {
                claims.Add(CreateEntry<T>((T)obj, false));
            }
        }

        return claims;
    }

    public async ValueTask<Claim<T>> LoadNamed<T>(string name)
        where T : notnull, IIdentifiable
        => await this.Load<T>(CreateId(name));

    public async ValueTask Delete(IEnumerable<string> ids)
        => await store.Delete(ids);

    public void Mark(IEnumerable<string> ids)
    {
        foreach (string id in ids)
        {
            if (entries.TryGetValue(id, out Entry? entry))
            {
                entry.Mark();
            }
        }
    }

    public async ValueTask Save()
        => await store.Save(entries.Values.Where(x => x.IsMarked).Select(x => x.Clean()).ToList());

    private string CreateId(string? name)
        => name != null ? "@" + name : "#" + idGenerator.GenerateId();

    private Claim<T> CreateEntry<T>(T obj, bool mark)
        where T : IIdentifiable
    {
        Entry entry = new(obj);
        entries.Add(entry.Id, entry);
        
        if (mark)
        {
            entry.Mark();
        }

        return entry.Claim<T>();
    }

    private class Entry
    {
        public Entry(IIdentifiable instance)
        {
            Instance = instance;
        }

        public IIdentifiable Instance { get; }

        public int Claims { get; private set; }
        public bool IsMarked { get; private set; }

        public string Id => Instance.Id;

        public Claim<T> Claim<T>()
            where T : notnull
        {
            Claims++;
            return new Claim<T>((T)Instance, () => Claims--);
        }

        public void Mark()
            => IsMarked = true;

        public IIdentifiable Clean()
        {
            IsMarked = false;
            return Instance;
        }
    }
}