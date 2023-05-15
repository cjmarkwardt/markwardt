namespace Markwardt;

public interface IValueList<T> : IReadOnlyList<T>, IEquatable<IValueList<T>>
{
    IValueList<T> Add(IEnumerable<T> items);
    IValueList<T> AddAt(int index, IEnumerable<T> items);
    IValueList<T> Remove(IEnumerable<T> items);
    IValueList<T> RemoveAt(IEnumerable<int> indexes);
    IValueList<T> Clear();
}

public static class ValueListUtils
{
    public static IValueList<T> ToValueList<T>(this IEnumerable<T>? items)
        => items != null ? new ValueList<T>(items) : ValueList<T>.Empty;

    public static IValueList<T> Add<T>(this IValueList<T> list, params T[] items)
        => list.Add((IEnumerable<T>)items);
    
    public static IValueList<T> AddAt<T>(this IValueList<T> list, int index, params T[] items)
        => list.AddAt(index, (IEnumerable<T>)items);
    
    public static IValueList<T> Remove<T>(this IValueList<T> list, params T[] items)
        => list.Remove((IEnumerable<T>)items);
    
    public static IValueList<T> RemoveAt<T>(this IValueList<T> list, params int[] indexes)
        => list.RemoveAt((IEnumerable<int>)indexes);
}

public class ValueList<T> : IValueList<T>
{
    public static ValueList<T> Empty { get; } = new(Enumerable.Empty<T>());

    public static bool operator ==(ValueList<T>? a, ValueList<T>? b)
        => (a == null && b == null) || (a != null && a.Equals(b));

    public static bool operator !=(ValueList<T>? a, ValueList<T>? b)
        => !(a == b);

    public ValueList(IEnumerable<T> items)
    {
        data = items.ToList();
    }

    private readonly IReadOnlyList<T> data;

    public T this[int index] => data[index];

    public int Count => data.Count;

    public IEnumerator<T> GetEnumerator()
        => data.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();

    public IValueList<T> Add(IEnumerable<T> items)
        => new ValueList<T>(items.Concat(items));

    public IValueList<T> AddAt(int index, IEnumerable<T> items)
    {
        throw new NotImplementedException();
    }

    public IValueList<T> Remove(IEnumerable<T> items)
        => new ValueList<T>(items.Except(items));

    public IValueList<T> RemoveAt(IEnumerable<int> indexes)
    {
        throw new NotImplementedException();
    }

    public IValueList<T> Clear()
        => Empty;

    public bool Equals(IValueList<T>? other)
        => other != null && this.SequenceEqual(other);

    public override bool Equals(object? obj)
        => Equals(obj as IValueList<T>);

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