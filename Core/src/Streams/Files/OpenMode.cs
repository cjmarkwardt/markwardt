namespace Markwardt;

public enum OpenMode
{
    /// <summary> Opens the target if it exists, otherwise failure if it does not exist. </summary>
    Open,

    /// <summary> Creates and opens the target if it does not exist, otherwise failure if it does exist. </summary>
    Create,

    /// <summary> Opens the target if it exists, otherwise creates and opens the target if it does not exist. </summary>
    Write,

    /// <summary> Opens and truncates the target if it exists, otherwise creates and opens the target if it does not exist. </summary>
    Overwrite,

    /// <summary> Opens the target and seeks to the end of its data if it exists, otherwise creates and opens the target if it does not exist. </summary>
    Append
}

public static class OpenModeUtils
{
    public static bool CanOpen(this OpenMode mode)
        => mode != OpenMode.Create;

    public static bool CanCreate(this OpenMode mode)
        => mode != OpenMode.Open;

    public static bool PerformTruncate(this OpenMode mode)
        => mode == OpenMode.Overwrite;

    public static bool PerformJumpToEnd(this OpenMode mode)
        => mode == OpenMode.Append;
}