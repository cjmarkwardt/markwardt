namespace Markwardt;

public interface IDataSource
{
    ValueTask<Failable<Stream>> Open(CancellationToken cancellation = default);
}