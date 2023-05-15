namespace Markwardt;

public static class DebugTimer
{
    private static readonly System.Diagnostics.Stopwatch watch = new();

    public static void Start()
        => watch.Start();

    public static void Log(string message)
        => Console.WriteLine($"({watch.Elapsed}) {message}");
}