namespace Markwardt;

public interface IValueSet<T> : IEnumerable<T>, IEquatable<IValueSet<T>>
{
    IValueSet<T> Add(IEnumerable<T> items);
    IValueSet<T> Remove(IEnumerable<T> items);
    IValueSet<T> Clear();
}

public class ValueSet<T> : IValueSet<T>
{
    public static ValueSet<T> Empty { get; } = new ValueSet<T>(Enumerable.Empty<T>());

    public ValueSet(IEnumerable<T> items)
    {
        data = items.ToHashSet();
    }

    private readonly HashSet<T> data;

    public int Count => data.Count;

    public IValueSet<T> Add(IEnumerable<T> items)
        => new ValueSet<T>(data.Concat(items));

    public IValueSet<T> Clear()
        => Empty;

    public bool Contains(T item)
        => data.Contains(item);

    public IEnumerator<T> GetEnumerator()
        => data.GetEnumerator();

    public bool IsProperSubsetOf(IEnumerable<T> other)
        => data.IsProperSubsetOf(other);

    public bool IsProperSupersetOf(IEnumerable<T> other)
        => data.IsProperSupersetOf(other);

    public bool IsSubsetOf(IEnumerable<T> other)
        => data.IsSubsetOf(other);

    public bool IsSupersetOf(IEnumerable<T> other)
        => data.IsSupersetOf(other);

    public bool Overlaps(IEnumerable<T> other)
        => data.Overlaps(other);

    public IValueSet<T> Remove(IEnumerable<T> items)
        => new ValueSet<T>(data.Except(items));

    public bool SetEquals(IEnumerable<T> other)
        => data.SetEquals(other);

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();

    public bool Equals(IValueSet<T>? other)
        => other != null && SetEquals(other);

    public override bool Equals(object? obj)
        => Equals(obj as IValueSet<T>);

    public override int GetHashCode()
    {
        HashCode hash = new();
        foreach (T item in this)
        {
            hash.Add(item);
        }

        return hash.ToHashCode();
    }
}