namespace Markwardt;

public abstract class CodedExiter : ServiceExiter
{
    protected CodedExiter(IObjectContainer services)
        : base(services) { }

    protected sealed override void ExecuteExit(object? info)
        => ExecuteCodedExit((info as int?) ?? 0);

    protected abstract void ExecuteCodedExit(int exitCode);
}