namespace Markwardt;

public static class DisposableUtils
{
    public static void TryDispose(this object? obj)
    {
        if (obj is IDisposable disposable)
        {
            disposable.Dispose();
        }
    }

    public static async ValueTask TryDisposeAsync(this object? obj)
    {
        if (obj is IAsyncDisposable asyncDisposable)
        {
            await asyncDisposable.DisposeAsync();
        }
        else
        {
            obj.TryDispose();
        }
    }

    public static void TryDisposeAll(this IEnumerable<object?> targets)
    {
        foreach (object? target in targets)
        {
            target.TryDispose();
        }
    }

    public static async Task TryDisposeAllAsync(this IEnumerable<object?> targets)
        => await Task.WhenAll(targets.Select(t => t.TryDisposeAsync().AsTask()));

    public static void DisposalAll<T>(this IEnumerable<T> targets)
        where T : IDisposable
    {
        foreach (T target in targets)
        {
            target.Dispose();
        }
    }

    public static async Task DisposeAllAsync<T>(this IEnumerable<T> targets)
        where T : IAsyncDisposable
        => await Task.WhenAll(targets.Select(x => x.DisposeAsync().AsTask()));
}