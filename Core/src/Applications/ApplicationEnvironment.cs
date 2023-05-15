namespace Markwardt;

public interface IApplicationEnvironment
{
    ValueTask<Failable> Run(AsyncFunc<Failable> start);
    void Configure(IServiceConfiguration services);
}

public abstract class ApplicationEnvironment : IApplicationEnvironment
{
    public virtual void Configure(IServiceConfiguration services) { }

    public virtual async ValueTask<Failable> Run(AsyncFunc<Failable> start)
        => await start();
}