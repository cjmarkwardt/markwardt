namespace Markwardt;

public interface IValueDictionary<TKey, TValue> : IReadOnlyDictionary<TKey, TValue>, IEquatable<IValueDictionary<TKey, TValue>>
{
    IValueDictionary<TKey, TValue> Add(IEnumerable<KeyValuePair<TKey, TValue>> pairs);
    IValueDictionary<TKey, TValue> Replace(IEnumerable<KeyValuePair<TKey, TValue>> pairs);
    IValueDictionary<TKey, TValue> Remove(IEnumerable<TKey> keys);
    IValueDictionary<TKey, TValue> Clear();
}

public static class ValueDictionaryUtils
{
    public static IValueDictionary<TKey, TValue> ToValueDictionary<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>>? pairs)
        where TKey : notnull
        => pairs != null ? new ValueDictionary<TKey, TValue>(pairs) : ValueDictionary<TKey, TValue>.Empty;

    public static IValueDictionary<TKey, TValue> ToValueDictionary<T, TKey, TValue>(this IEnumerable<T>? items, Func<T, TKey> keySelector, Func<T, TValue> valueSelector)
        where TKey : notnull
        => items == null ? ValueDictionary<TKey, TValue>.Empty : new ValueDictionary<TKey, TValue>(items.Select(i => new KeyValuePair<TKey, TValue>(keySelector(i), valueSelector(i))));

    public static IValueDictionary<TKey, T> ToValueDictionary<T, TKey>(this IEnumerable<T>? items, Func<T, TKey> keySelector)
        where TKey : notnull
        => items.ToValueDictionary<T, TKey, T>(keySelector, v => v);
}

public class ValueDictionary<TKey, TValue> : IValueDictionary<TKey, TValue>
    where TKey : notnull
{
    public static ValueDictionary<TKey, TValue> Empty { get; } = new(Enumerable.Empty<KeyValuePair<TKey, TValue>>());

    public ValueDictionary(IEnumerable<KeyValuePair<TKey, TValue>> pairs)
    {
        Dictionary<TKey, TValue> data = new();
        foreach (KeyValuePair<TKey, TValue> pair in pairs)
        {
            data[pair.Key] = pair.Value;
        }

        this.data = data;
    }

    private readonly IReadOnlyDictionary<TKey, TValue> data;

    public TValue this[TKey key] => data[key];

    public IEnumerable<TKey> Keys => data.Keys;

    public IEnumerable<TValue> Values => data.Values;

    public int Count => data.Count;

    public IValueDictionary<TKey, TValue> Add(IEnumerable<KeyValuePair<TKey, TValue>> pairs)
    {
        if (pairs.Any(p => data.ContainsKey(p.Key)))
        {
            throw new InvalidOperationException();
        }

        return Replace(pairs);
    }

    public IValueDictionary<TKey, TValue> Clear()
        => Empty;

    public bool ContainsKey(TKey key)
        => data.ContainsKey(key);

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        => data.GetEnumerator();

    public IValueDictionary<TKey, TValue> Remove(IEnumerable<TKey> keys)
        => new ValueDictionary<TKey, TValue>(data.Where(p => keys.Any(k => !p.Key.Equals(k))));

    public IValueDictionary<TKey, TValue> Replace(IEnumerable<KeyValuePair<TKey, TValue>> pairs)
        => new ValueDictionary<TKey, TValue>(GetReplacedPairs(pairs));

    public bool TryGetValue(TKey key, [NotNullWhen(true)] out TValue value)
        => data.TryGetValue(key, out value);

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();

    private IEnumerable<KeyValuePair<TKey, TValue>> GetReplacedPairs(IEnumerable<KeyValuePair<TKey, TValue>> replacements)
    {
        foreach (KeyValuePair<TKey, TValue> replacement in replacements)
        {
            if (!ContainsKey(replacement.Key))
            {
                yield return replacement;
            }
        }

        foreach (KeyValuePair<TKey, TValue> pair in this)
        {
            yield return pair;
        }
    }

    public bool Equals(IValueDictionary<TKey, TValue>? other)
    {
        if (other == null || Keys.Except(other.Keys).Any())
        {
            return false;
        }
        
        foreach (KeyValuePair<TKey, TValue> pair in this)
        {
            if (!pair.Value.NullableEquals(other[pair.Key]))
            {
                return false;
            }
        }

        return true;
    }

    public override bool Equals(object? obj)
        => Equals(obj as IValueDictionary<TKey, TValue>);

    public override int GetHashCode()
    {
        HashCode hash = new();
        foreach (KeyValuePair<TKey, TValue> pair in this)
        {
            hash.Add(pair.Key);
            hash.Add(pair.Value);
        }

        return hash.ToHashCode();
    }
}