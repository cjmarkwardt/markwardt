namespace Markwardt;

public static class AsyncOperationUtils
{
    public static Task AsTask(this AsyncOperation operation)
    {
        if (operation.isDone)
        {
            return Task.CompletedTask;
        }

        TaskCompletionSource<bool> completion = new();

        void OnCompleted(AsyncOperation o)
        {
            completion.SetResult(true);
            operation.completed -= OnCompleted;
        }

        operation.completed += OnCompleted;
        return completion.Task;
    }

    public static TaskAwaiter GetAwaiter(this AsyncOperation operation)
        => operation.AsTask().GetAwaiter();

    public static Task<Object?> AsTask(this ResourceRequest request)
    {
        if (request.isDone)
        {
            return Task.FromResult<Object?>(request.asset);
        }

        TaskCompletionSource<Object?> completion = new();

        void OnCompleted(AsyncOperation o)
        {
            completion.SetResult(request.asset);
            request.completed -= OnCompleted;
        }

        request.completed += OnCompleted;
        return completion.Task;
    }

    public static async Task<T?> AsTask<T>(this ResourceRequest request)
        where T : Object
        => (T?) await request;

    public static TaskAwaiter<Object?> GetAwaiter(this ResourceRequest request)
        => request.AsTask().GetAwaiter();
}