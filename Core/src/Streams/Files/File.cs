namespace Markwardt;

public record File(IFileSystem FileSystem, Path Path) : FileNode(FileSystem, Path)
{
    public async ValueTask<Failable<Stream>> Open(OpenMode mode = OpenMode.Open, CancellationToken cancellation = default)
        => await FileSystem.Open(Path, mode, cancellation);

    public async ValueTask<Failable<Stream>> View(CancellationToken cancellation = default)
        => await FileSystem.View(Path, cancellation);

    public FileOpenSource AsOpenSource(OpenMode mode = OpenMode.Open)
        => new FileOpenSource(this, mode);

    public FileViewSource AsViewSource()
        => new FileViewSource(this);
}