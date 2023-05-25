namespace Markwardt;

public interface IDataCacheAccessor<in TKey> : IFullDisposable
{
    ValueTask<Failable<bool>> Exists(TKey key, CancellationToken cancellation = default);
    ValueTask<Failable<Stream>> Open(TKey key, OpenMode mode = OpenMode.Open, CancellationToken cancellation = default);
    ValueTask<Failable<bool>> Delete(TKey key, CancellationToken cancellation = default);
}