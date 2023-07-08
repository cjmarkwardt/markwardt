namespace Markwardt;

public interface ICompositeAsyncDisposable : IAsyncDisposable, ICompositeDisposable { }

public static class CompositeAsyncDisposableUtils
{
    public static T DisposeWith<T>(this T target, ICompositeAsyncDisposable tracker)
    {
        tracker.Add(target);
        return target;
    }
}