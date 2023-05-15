namespace Markwardt;

public class SimpleLogFormatter : ILogFormatter
{
    public string Format(LogMessage message)
        => $"[{FormatTime(message.Time)}] [{FormatType(message.Type)}] [{FormatFile(message.File, message.Line)}]\n    {FormatTarget(message.Target)}";

    private string FormatTime(DateTime time)
        => $"{time.ToShortDateString()}";

    private string FormatType(object type)
        => $"{type.ToString() ?? string.Empty}";

    private string FormatFile(string file, int line)
        => $"{file.Split('\\').Last()}:{line}";

    private string FormatTarget(object target)
        => string.Join("\n    ", (target.ToString() ?? target.GetType().Name).Replace("\r\n", "\n").Split('\n'));
}