namespace Markwardt;

public record Folder(IFileSystem FileSystem, Path Path) : FileNode(FileSystem, Path)
{
    public IAsyncEnumerable<Failable<FileNode>> Scan(bool recursive = false, int? batch = null, CancellationToken cancellation = default)
        => FileSystem.Scan(Path, recursive, batch, cancellation);
}