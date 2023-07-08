namespace Markwardt;

public interface IApplication
{
    ValueTask<Failable> TryRun(IEnumerable<string> arguments, IServicePackage? package = null);
}

public static class ApplicationUtils
{
    public static async ValueTask<Failable> TryRun(this IApplication application, IServicePackage? package = null)
        => await application.TryRun(Enumerable.Empty<string>(), package);

    public static async ValueTask<Failable> TryRunWithCommandLineArguments(this IApplication application, IServicePackage? package = null)
        => await application.TryRun(Environment.GetCommandLineArgs(), package);

    public static async ValueTask Run(this IApplication application, IEnumerable<string> arguments, IServicePackage? package = null)
        => (await application.TryRun(arguments, package)).Verify();

    public static async ValueTask Run(this IApplication application, IServicePackage? package = null)
        => (await application.TryRun(package)).Verify();

    public static async ValueTask RunWithCommandLineArguments(this IApplication application, IServicePackage? package = null)
        => (await application.TryRunWithCommandLineArguments(package)).Verify();
}

public abstract class Application<TStarter> : IApplication
    where TStarter : class, IApplicationStarter
{
    public async ValueTask<Failable> TryRun(IEnumerable<string> arguments, IServicePackage? package = null)
    {
        IServiceContainer container = new ServiceContainer();
        container.ConfigureInstance<IApplicationArguments>(new ApplicationArguments(arguments.ToList()));
        container.Configure(package);
        Configure(container);
        TStarter starter = await container.Resolve<TStarter>();
        return await starter.Start();
    }

    protected virtual void Configure(IServiceContainer services) { }
}