namespace Markwardt;

[SubstituteAs<IConsole>]
public interface IConsoleReader
{
    ValueTask<string> Read(object? question = null);
}