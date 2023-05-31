namespace Markwardt;

public static class DictionaryUtils
{
    public static TValue? TryGetValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
        => dictionary.TryGetValue(key, out TValue? value) ? value : default;
}