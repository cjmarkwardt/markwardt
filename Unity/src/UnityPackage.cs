namespace Markwardt;

public class UnityPackage : IServicePackage
{
    public void Configure(IServiceConfiguration services)
    {
        services.Configure<IConsoleWriter, UnityConsoleWriter>();
        services.Configure<IExiter, UnityExiter>();
    }
}