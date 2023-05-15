namespace Markwardt;

public record LogMessage(DateTime Time, object Type, object Target, string File, int Line)
{
    public CommonLogType? CommonType => Type as CommonLogType?;
}