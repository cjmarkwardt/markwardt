namespace Markwardt;

public interface IEntitySystem
{
    T Create<T>(string? name, Func<string, T> create)
        where T : notnull, IEntity;

    ValueTask<IEnumerable<T>> Load<T>(IEnumerable<string> ids)
        where T : notnull, IEntity;

    ValueTask<T> LoadNamed<T>(string name)
        where T : notnull, IEntity;

    ValueTask Delete(IEnumerable<string> ids);

    void Mark(IEnumerable<string> ids);

    ValueTask Save();
}

public static class EntitySystemUtils
{
    public static T Create<T>(this IEntitySystem system, Func<string, T> create)
        where T : notnull, IEntity
        => system.Create(null, create);

    public static async ValueTask<IEnumerable<T>> Load<T>(this IEntitySystem system, IEnumerable<Id<T>> ids)
        where T : notnull, IEntity
        => await system.Load<T>(ids.Select(x => x.Value));

    public static async ValueTask<T> Load<T>(this IEntitySystem system, string id)
        where T : notnull, IEntity
        => (await system.Load<T>(Enumerable.Repeat(id, 1))).First();

    public static async ValueTask<T> Load<T>(this IEntitySystem system, Id<T> id)
        where T : notnull, IEntity
        => await system.Load<T>(id.Value);

    public static async ValueTask Delete(this IEntitySystem system, string id)
        => await system.Delete(Enumerable.Repeat(id, 1));

    public static void Mark(this IEntitySystem system, string id)
        => system.Mark(Enumerable.Repeat(id, 1));
}

public class EntitySystem : IEntitySystem
{
    public EntitySystem(IEntityClaimSystem system)
    {
        this.system = system;
    }

    private readonly IEntityClaimSystem system;

    public T Create<T>(string? name, Func<string, T> create)
        where T : notnull, IEntity
        => Wrap(system.Create<T>(name, create));

    public async ValueTask Delete(IEnumerable<string> ids)
        => await system.Delete(ids);

    public async ValueTask<IEnumerable<T>> Load<T>(IEnumerable<string> ids)
        where T : notnull, IEntity
        => (await system.Load<T>(ids)).Select(x => Wrap(x)).ToList();

    public async ValueTask<T> LoadNamed<T>(string name)
        where T : notnull, IEntity
        => Wrap(await system.LoadNamed<T>(name));

    public void Mark(IEnumerable<string> ids)
        => system.Mark(ids);

    public async ValueTask Save()
        => await system.Save();

    private T Wrap<T>(Claim<T> claim)
        where T : notnull, IEntity
    {
        claim.Instance.Add(claim);
        return claim.Instance;
    }
}