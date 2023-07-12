namespace Markwardt;

public interface IOperationQueue
{
    Task Completion { get; }

    Task<TResult> Enqueue<TResult>(Func<Task<TResult>> action);
}

public static class OperationQueueUtils
{
    public static async Task Enqueue(this IOperationQueue queue, Func<Task> action)
        => await queue.Enqueue<bool>(async () => { await action(); return false; });

    public static async Task<TResult> Enqueue<TResult>(this IOperationQueue queue, Func<TResult> action)
        => await queue.Enqueue<TResult>(() => Task.FromResult(action()));

    public static async Task Enqueue(this IOperationQueue queue, Action action)
        => await queue.Enqueue<bool>(() => { action(); return false; });

    public static async Task EnqueueThread(this IOperationQueue queue, Func<Task> action)
        => await queue.Enqueue<bool>(async () => { await Task.Run(action); return false; });

    public static async Task<TResult> EnqueueThread<TResult>(this IOperationQueue queue, Func<Task<TResult>> action)
        => await queue.Enqueue<TResult>(() => Task.Run(action));

    public static async Task<TResult> EnqueueThread<TResult>(this IOperationQueue queue, Func<TResult> action)
        => await queue.Enqueue<TResult>(() => Task.Run(action));

    public static async Task EnqueueThread(this IOperationQueue queue, Action action)
        => await queue.Enqueue<bool>(async () => { await Task.Run(action); return false; });
}

public class OperationQueue : IOperationQueue
{
    public OperationQueue()
    {
        completion.SetResult(false);
    }

    private readonly Queue<IOperation> operations = new();

    private TaskCompletionSource<bool> completion = new();

    public Task Completion => completion.Task;

    public async Task<TResult> Enqueue<TResult>(Func<Task<TResult>> action)
    {
        Operation<TResult> operation = new(action);
        operations.Enqueue(operation);

        if (operations.Count == 1)
        {
            completion = new();
            operation.Start();
        }

        TResult result = await operation.Completion;
        operations.Dequeue();

        if (operations.Any())
        {
            operations.Peek().Start();
        }
        else
        {
            completion.SetResult(false);
        }

        return result;
    }

    private interface IOperation
    {
        void Start();
    }

    private record Operation<TResult>(Func<Task<TResult>> Action) : IOperation
    {
        private TaskCompletionSource<TResult> completion = new();
        public Task<TResult> Completion => completion.Task;

        public async void Start()
            => completion.SetResult(await Action());
    }
}