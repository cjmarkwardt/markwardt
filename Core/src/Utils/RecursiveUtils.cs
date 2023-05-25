namespace Markwardt;

public static class RecursiveUtils
{
    public static IEnumerable<T> Chain<T>(T start, bool includeStart, Func<T, IEnumerable<T>> getNext)
    {
        if (includeStart)
        {
            yield return start;
        }

        foreach (T node in getNext(start).SelectMany(x => Chain(x, true, getNext)))
        {
            yield return node;
        }
    }

    public static IEnumerable<T> Chain<T>(IEnumerable<T> starts, bool includeStarts, Func<T, IEnumerable<T>> getNext)
    {
        foreach (T start in starts)
        {
            foreach (T node in Chain(start, includeStarts, getNext))
            {
                yield return node;
            }
        }
    }

    public static IEnumerable<T> Chain<T>(T start, bool includeStart, Func<T, bool> hasNext, Func<T, T> getNext)
        => Chain(start, includeStart, x => hasNext(x) ? new T[] { getNext(x) } : Enumerable.Empty<T>());

    public static IEnumerable<T> Chain<T>(T start, bool includeStart, Func<T, T?> getNext)
        where T : class
        => Chain(start, includeStart, x => getNext(x) != null, x => getNext(x).NotNull());
}