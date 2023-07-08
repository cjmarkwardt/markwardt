namespace Markwardt;

public class OverriddenDictionary<TKey, TValue> : IReadOnlyDictionary<TKey, TValue>
{
    public OverriddenDictionary(IReadOnlyDictionary<TKey, TValue> source, IReadOnlyDictionary<TKey, TValue> overrides)
    {
        this.source = source;
        this.overrides = overrides;
    }

    private readonly IReadOnlyDictionary<TKey, TValue> source;
    private readonly IReadOnlyDictionary<TKey, TValue> overrides;

    public TValue this[TKey key] => TryGetValue(key, out TValue? value) ? value : throw new KeyNotFoundException($"Key {key} not found");

    public IEnumerable<TKey> Keys => overrides.Keys.Concat(source.Keys.Except(overrides.Keys));

    public IEnumerable<TValue> Values => GetValues();

    public int Count => Keys.Count();

    public bool ContainsKey(TKey key)
        => overrides.ContainsKey(key) ? true : source.ContainsKey(key);

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
    {
        foreach (TKey key in Keys)
        {
            yield return new KeyValuePair<TKey, TValue>(key, this[key]);
        }
    }

    public bool TryGetValue(TKey key, out TValue value)
        => overrides.TryGetValue(key, out value) ? true : source.TryGetValue(key, out value);

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();

    private IEnumerable<TValue> GetValues()
    {
        foreach (KeyValuePair<TKey, TValue> pair in this)
        {
            yield return pair.Value;
        }
    }
}