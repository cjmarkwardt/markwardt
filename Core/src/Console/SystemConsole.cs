namespace Markwardt;

[Singleton<SystemConsole>]
public interface ISystemConsole : IConsoleReader
{
    void WriteColored(object? target, ConsoleColor color, bool writeLine = true);
}

public class SystemConsole : ConsoleWriter, ISystemConsole
{
    public void WriteColored(object? target, ConsoleColor color, bool writeLine = true)
    {
        if (target != null)
        {
            using (Colorize(color))
            {
                Console.Write(target);

                if (writeLine)
                {
                    Console.WriteLine();
                }
            }
        }
    }

    public async ValueTask<string> Read(object? question = null)
    {
        WriteColored(question, ConsoleColor.Yellow, false);
        using (Colorize(ConsoleColor.Green))
        {
            return await Task.Run(() => Console.ReadLine());
        }
    }

    protected override void WriteTarget(object target, bool isError = false)
        => WriteColored(target, isError ? ConsoleColor.Red : ConsoleColor.White);

    private IDisposable Colorize(ConsoleColor color)
    {
        ConsoleColor oldColor = Console.ForegroundColor;
        Console.ForegroundColor = color;
        return new Disposable(() => Console.ForegroundColor = oldColor);
    }
}