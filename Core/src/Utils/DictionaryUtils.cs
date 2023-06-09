namespace Markwardt;

public static class DictionaryUtils
{
    public static Maybe<TValue> TryGetValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
        => dictionary.TryGetValue(key, out TValue? value) ? value : default(Maybe<TValue>);
}