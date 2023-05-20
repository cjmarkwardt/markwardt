namespace Markwardt;

public record FileViewSource(File File) : IDataSource
{
    public async ValueTask<Failable<Stream>> Open(CancellationToken cancellation = default)
        => await File.View(cancellation);
}