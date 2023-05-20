namespace Markwardt;

public class MemoryCache<TKey> : ManagedAsyncDisposable, IDataCache<TKey>
{
    private readonly Dictionary<TKey, MemoryStream> streams = new();

    public async ValueTask<Failable<bool>> Delete(TKey key, CancellationToken cancellation = default)
    {
        if (streams.TryGetValue(key, out MemoryStream? stream))
        {
            await stream.DisposeAsync();
            streams.Remove(key);
            return true;
        }
        else
        {
            return false;
        }
    }

    public ValueTask<Failable<bool>> Exists(TKey key, CancellationToken cancellation = default)
        => Failable.Success(streams.ContainsKey(key));

    public async IAsyncEnumerable<Failable<TKey>> GetKeys(int? batch = null, [EnumeratorCancellation] CancellationToken cancellation = default)
    {
        foreach (TKey key in streams.Keys)
        {
            yield return key;
        }

        await new ValueTask();
    }

    public ValueTask<Failable<Stream>> Open(TKey key, OpenMode mode = OpenMode.Open, CancellationToken cancellation = default)
    {
        if (!streams.TryGetValue(key, out MemoryStream? stream))
        {
            if (!mode.CanCreate())
            {
                return new Failure($"Key {key} not found in cache").AsFailable<Stream>();
            }

            stream = new();
            streams.Add(key, stream);
        }
        else
        {
            if (!mode.CanOpen())
            {
                return new Failure($"Key {key} already exists in cache").AsFailable<Stream>();
            }
        }

        if (mode.PerformTruncate())
        {
            stream.SetLength(0);
        }

        if (mode.PerformJumpToEnd())
        {
            stream.Seek(0, SeekOrigin.End);
        }

        return stream.AsFailable<Stream>();
    }
}