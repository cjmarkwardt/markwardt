namespace Markwardt;

public class DefaultDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    where TKey : notnull
{
    public DefaultDictionary(Func<TValue> createDefault)
    {
        this.createDefault = createDefault;
        data = new Dictionary<TKey, TValue>();
    }

    private readonly Func<TValue> createDefault;
    private readonly IDictionary<TKey, TValue> data;

    public TValue this[TKey key]
    {
        get
        {
            if (!data.TryGetValue(key, out TValue? value))
            {
                value = createDefault();
                data.Add(key, value);
            }

            return value;
        }
        set => data[key] = value;
    }

    public ICollection<TKey> Keys => data.Keys;

    public ICollection<TValue> Values => data.Values;

    public int Count => data.Count;

    public bool IsReadOnly => data.IsReadOnly;

    public void Add(TKey key, TValue value)
    {
        data.Add(key, value);
    }

    public void Add(KeyValuePair<TKey, TValue> item)
    {
        data.Add(item);
    }

    public void Clear()
    {
        data.Clear();
    }

    public bool Contains(KeyValuePair<TKey, TValue> item)
    {
        return data.Contains(item);
    }

    public bool ContainsKey(TKey key)
    {
        return data.ContainsKey(key);
    }

    public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
    {
        data.CopyTo(array, arrayIndex);
    }

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
    {
        return data.GetEnumerator();
    }

    public bool Remove(TKey key)
    {
        return data.Remove(key);
    }

    public bool Remove(KeyValuePair<TKey, TValue> item)
    {
        return data.Remove(item);
    }

    public bool TryGetValue(TKey key, [NotNullWhen(true)] out TValue value)
    {
        return data.TryGetValue(key, out value);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)data).GetEnumerator();
    }
}