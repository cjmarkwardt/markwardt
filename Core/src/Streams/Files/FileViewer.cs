namespace Markwardt;

[RoutedSingleton<IFileSystem>]
public interface IFileViewer
{
    ValueTask<Failable<bool>> Exists(Path path, CancellationToken cancellation = default);

    ValueTask<Failable<Stream>> View(Path path, CancellationToken cancellation = default);
}