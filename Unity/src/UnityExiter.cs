namespace Markwardt;

public class UnityExiter : CodedExiter
{
    public UnityExiter(IObjectContainer services)
        : base(services) { }

    protected override void ExecuteCodedExit(int exitCode)
        => Application.Quit(exitCode);
}