namespace Markwardt;

public record FileNode(IFileSystem FileSystem, Path Path)
{
    public FileNode GetNode(Path path)
        => FileSystem.GetNode(Path + path);

    public File GetFile(Path path)
        => FileSystem.GetFile(Path + path);

    public Folder GetFolder(Path path)
        => FileSystem.GetFolder(Path + path);

    public File AsFile()
        => FileSystem.GetFile(Path);

    public Folder AsFolder()
        => FileSystem.GetFolder(Path);

    public async ValueTask<Failable<bool>> IsFile(CancellationToken cancellation = default)
        => await FileSystem.IsFile(Path, cancellation);

    public async ValueTask<Failable<bool>> IsFolder(CancellationToken cancellation = default)
        => await FileSystem.IsFolder(Path, cancellation);

    public async ValueTask<Failable<bool>> Exists(CancellationToken cancellation = default)
        => await FileSystem.Exists(Path, cancellation);

    public async ValueTask<Failable> Delete(CancellationToken cancellation = default)
        => await FileSystem.Delete(Path, cancellation);
}