namespace Markwardt;

public static class ValueTaskUtils
{
    public static async void Fork(this ValueTask task)
        => await task;

    public static void Fork(this Func<ValueTask> task)
        => task().Fork();
}