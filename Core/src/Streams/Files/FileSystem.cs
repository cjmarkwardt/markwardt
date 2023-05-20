namespace Markwardt;

[RoutedSingleton<ILocalSystem>]
public interface IFileSystem : IFileViewer
{
    FileNode GetNode(Path path);
    File GetFile(Path path);
    Folder GetFolder(Path path);

    ValueTask<Failable<bool>> IsFile(Path path, CancellationToken cancellation = default);
    ValueTask<Failable<bool>> IsFolder(Path path, CancellationToken cancellation = default);
    
    IAsyncEnumerable<Failable<FileNode>> Scan(Path path, bool recursive = false, int? batch = null, CancellationToken cancellation = default);

    ValueTask<Failable<Stream>> Open(Path path, OpenMode mode = OpenMode.Open, CancellationToken cancellation = default);
    ValueTask<Failable<bool>> Delete(Path path, CancellationToken cancellation = default);
}