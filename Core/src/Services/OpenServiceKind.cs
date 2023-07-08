namespace Markwardt;

public enum OpenServiceKind
{
    Natural,
    Transient,
    Singleton
}

public static class OpenServiceKindUtils
{
    public static ServiceKind Close(this OpenServiceKind kind, ServiceKind naturalKind)
        => kind switch
        {
            OpenServiceKind.Natural => naturalKind,
            OpenServiceKind.Transient => ServiceKind.Transient,
            OpenServiceKind.Singleton => ServiceKind.Singleton,
            _ => throw new InvalidOperationException()
        };
}