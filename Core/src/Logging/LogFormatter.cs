namespace Markwardt;

[Singleton<SimpleLogFormatter>]
public interface ILogFormatter
{
    string Format(LogMessage message);
}