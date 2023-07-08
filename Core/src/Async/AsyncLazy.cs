namespace Markwardt;

public class AsyncLazy<T> : ManagedAsyncDisposable
{
    public AsyncLazy(AsyncFunc<T> create)
    {
        this.create = create;
    }

    public AsyncLazy(T value)
        : this(() => new ValueTask<T>(value)) { }

    public bool IsValueCreationStarted => task != null;
    public bool IsValueCreating => task != null && !task.IsCompleted;
    public bool IsValueCreated => task != null && task.IsCompleted;

    private readonly AsyncFunc<T> create;

    private Task<T>? task;

    public async ValueTask<T> GetValue()
    {
        Disposal.Verify();

        if (task == null)
        {
            task = create().AsTask();
        }

        return await task;
    }

    protected override void OnDisposal()
    {
        base.OnDisposal();

        if (task != null && task.IsCompleted)
        {
            task.Result.TryDispose();
        }
    }

    protected override async ValueTask OnAsyncDisposal()
    {
        await base.OnAsyncDisposal();

        if (task != null)
        {
            await (await GetValue()).TryDisposeAsync();
        }
    }
}