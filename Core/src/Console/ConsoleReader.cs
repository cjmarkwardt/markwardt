namespace Markwardt;

[RoutedService<IConsole>]
public interface IConsoleReader
{
    ValueTask<string> Read(object? question = null);
}