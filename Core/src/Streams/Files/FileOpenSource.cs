namespace Markwardt;

public record FileOpenSource(File File, OpenMode Mode = OpenMode.Open) : IDataSource
{
    public async ValueTask<Failable<Stream>> Open(CancellationToken cancellation = default)
        => await File.Open(Mode, cancellation);
}