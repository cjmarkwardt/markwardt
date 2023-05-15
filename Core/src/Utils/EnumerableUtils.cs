namespace Markwardt;

public static class EnumerableUtils
{
    [return: NotNull]
    public static IEnumerable<T> WhereNotNull<T>(this IEnumerable<T?> enumerable)
        => enumerable.Where(x => x != null).Select(x => x!);
        
    public static IEnumerable<T> One<T>(T value)
    {
        yield return value;
    }

    public static IEnumerable<(T1 First, T2 Second)> Zip<T1, T2>(this IEnumerable<T1> enumerable, IEnumerable<T2> other)
    {
        IEnumerator<T1> firstEnumerator = enumerable.GetEnumerator();
        IEnumerator<T2> secondEnumerator = other.GetEnumerator();
        while (firstEnumerator.MoveNext() && secondEnumerator.MoveNext())
        {
            yield return (firstEnumerator.Current, secondEnumerator.Current);
        }
    }
}