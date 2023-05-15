namespace Markwardt;

[Singleton<Logger>]
public interface ILogger
{
    void Log(LogMessage message);
}

public static class LoggerUtils
{
    public static void Log(this ILogger logger, object type, object target, [CallerFilePath] string? file = null, [CallerLineNumber] int line = 0)
        => logger.Log(new LogMessage(DateTime.Now, type, target, file.NotNull(), line));

    public static void LogActivity(this ILogger logger, object target, [CallerFilePath] string? file = null, [CallerLineNumber] int line = 0)
        => logger.Log(CommonLogType.Activity, target, file, line);

    public static void LogDebug(this ILogger logger, object target, [CallerFilePath] string? file = null, [CallerLineNumber] int line = 0)
        => logger.Log(CommonLogType.Debug, target, file, line);
    
    public static void LogError(this ILogger logger, object target, [CallerFilePath] string? file = null, [CallerLineNumber] int line = 0)
        => logger.Log(CommonLogType.Error, target, file, line);
}

public record Logger(IConsoleWriter Reporter, ILogFormatter Formatter) : ILogger
{
    public void Log(LogMessage message)
        => Reporter.Write(Formatter.Format(message), message.CommonType == CommonLogType.Error);
}