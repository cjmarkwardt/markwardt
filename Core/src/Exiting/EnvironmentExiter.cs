namespace Markwardt;

public class EnvironmentExiter : CodedExiter
{
    public EnvironmentExiter(IObjectContainer services)
        : base(services) { }

    protected override void ExecuteCodedExit(int exitCode)
        => Environment.Exit(exitCode);
}