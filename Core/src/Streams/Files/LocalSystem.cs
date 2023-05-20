namespace Markwardt;

[Singleton<LocalSystem>]
public interface ILocalSystem : IFileSystem { }

public class LocalSystem : ILocalSystem
{
    public ValueTask<Failable<bool>> Delete(Path path, CancellationToken cancellation = default)
    {
        throw new NotImplementedException();
    }

    public ValueTask<Failable<bool>> Exists(Path path, CancellationToken cancellation = default)
    {
        throw new NotImplementedException();
    }

    public File GetFile(Path path)
    {
        throw new NotImplementedException();
    }

    public Folder GetFolder(Path path)
    {
        throw new NotImplementedException();
    }

    public FileNode GetNode(Path path)
    {
        throw new NotImplementedException();
    }

    public ValueTask<Failable<bool>> IsFile(Path path, CancellationToken cancellation = default)
    {
        throw new NotImplementedException();
    }

    public ValueTask<Failable<bool>> IsFolder(Path path, CancellationToken cancellation = default)
    {
        throw new NotImplementedException();
    }

    public ValueTask<Failable<Stream>> Open(Path path, OpenMode mode = OpenMode.Open, CancellationToken cancellation = default)
    {
        throw new NotImplementedException();
    }

    public IAsyncEnumerable<Failable<FileNode>> Scan(Path path, bool recursive = false, int? batch = null, CancellationToken cancellation = default)
    {
        throw new NotImplementedException();
    }

    public ValueTask<Failable<Stream>> View(Path path, CancellationToken cancellation = default)
    {
        throw new NotImplementedException();
    }
}