namespace Markwardt;

public interface IDataCache<TKey> : IDataCacheAccessor<TKey>
{
    IAsyncEnumerable<Failable<TKey>> GetKeys(int? batch = null, CancellationToken cancellation = default);
}