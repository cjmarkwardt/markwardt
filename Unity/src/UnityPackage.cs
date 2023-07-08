namespace Markwardt;

public class UnityPackage : IServicePackage
{
    public void Configure(IServiceContainer container)
    {
        container.ConfigureConstructor<IConsoleWriter, UnityConsoleWriter>();
        container.ConfigureConstructor<IExiter, UnityExiter>();
    }
}