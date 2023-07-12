namespace Markwardt;

[Transient<MemoryStore>]
public interface IEntityStore
{
    delegate IEntityStore FileFactory(Path path);

    ValueTask Save(IEnumerable<IIdentifiable> targets);
    ValueTask<IEnumerable<IIdentifiable>> Load(IEnumerable<string> ids);
    ValueTask Delete(IEnumerable<string> ids);
}