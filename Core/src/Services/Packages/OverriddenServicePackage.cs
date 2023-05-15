namespace Markwardt;

public static class OverriddenServicePackageUtils
{
    public static IServicePackage Override(this IServicePackage package, IServicePackage @override)
        => new OverriddenServicePackage(package, @override);

    public static IServicePackage Fallback(this IServicePackage package, IServicePackage fallback)
        => fallback.Override(package);
}

public class OverriddenServicePackage : IServicePackage
{
    public OverriddenServicePackage(IServicePackage source, IServicePackage @override)
    {
        this.source = source;
        this.@override = @override;
    }

    private readonly IServicePackage source;
    private readonly IServicePackage @override;

    public void Configure(IServiceConfiguration services)
    {
        source.Configure(services);
        @override.Configure(services);
    }
}