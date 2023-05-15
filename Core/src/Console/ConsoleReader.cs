namespace Markwardt;

[RoutedSingleton<IConsole>]
public interface IConsoleReader
{
    ValueTask<string> Read(object? question = null);
}