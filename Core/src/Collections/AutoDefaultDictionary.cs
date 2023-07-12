namespace Markwardt;

public class AutoDefaultDictionary<TKey, TValue> : DefaultDictionary<TKey, TValue>
    where TKey : notnull
    where TValue : new()
{
    public AutoDefaultDictionary()
        : base(() => new()) { }
}