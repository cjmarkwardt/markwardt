namespace Markwardt;

public interface IApplicationStarter
{
    ValueTask<Failable> Start();
}