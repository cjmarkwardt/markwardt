namespace Markwardt;

public interface ICompositeDisposable : IDisposable
{
    void Add(params object?[] targets);
    void Remove(params object?[] targets);
}

public static class CompositeDisposableUtils
{
    public static T DisposeWith<T>(this T target, ICompositeDisposable tracker)
    {
        tracker.Add(target);
        return target;
    }
}