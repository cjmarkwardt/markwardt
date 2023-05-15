namespace Markwardt;

[Singleton<DebugStatus>]
public interface IDebugStatus
{
    bool IsActive { get; }
}

public class DebugStatus : IDebugStatus
{
    public static bool IsActive
    {
        get
        {
            #if DEBUG
                return true;
            #else
                return false;
            #endif
        }
    }

    bool IDebugStatus.IsActive => IsActive;
}