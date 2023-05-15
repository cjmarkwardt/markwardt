namespace Markwardt;

public class AsyncLazy<T>
{
    public AsyncLazy(AsyncFunc<T> create)
    {
        this.create = create;
    }

    public AsyncLazy(T value)
        : this(() => new ValueTask<T>(value)) { }

    public bool IsValueCreating => task != null && !task.IsCompleted;
    public bool IsValueCreated => task != null && task.IsCompleted;

    private readonly AsyncFunc<T> create;

    private Task<T>? task;

    public async ValueTask<T> GetValue()
    {
        if (task == null)
        {
            task = create().AsTask();
        }

        return await task;
    }
}