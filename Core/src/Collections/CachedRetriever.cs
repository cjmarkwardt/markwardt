namespace Markwardt;

public class CachedRetriever<TKey, TValue>
{
    public CachedRetriever(Func<TKey, TValue> retrieve)
    {
        this.retrieve = retrieve;
    }

    private readonly Func<TKey, TValue> retrieve;
    private readonly Dictionary<TKey, TValue> values = new();

    public TValue Get(TKey key)
    {
        if (!values.TryGetValue(key, out TValue? value))
        {
            value = retrieve(key);
            values.Add(key, value);
        }

        return value;
    }
}