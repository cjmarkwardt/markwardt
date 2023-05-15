namespace Markwardt;

[Singleton<AssetCatalog>]
public interface IAssetCatalog : IAssetFolder
{
    TimeSpan? AutoUnloadInterval { get; set; }

    Task<T> Load<T>(string path)
        where T : Object;

    void Unload(Object asset);

    void UnloadUnused();
}

public class AssetCatalog : IAssetCatalog
{
    public AssetCatalog()
    {
        AutoUnload();
    }
    
    private CancellationTokenSource autoUnloadDelay = new();

    private TimeSpan? autoUnloadInterval = TimeSpan.FromMinutes(5);
    public TimeSpan? AutoUnloadInterval
    {
        get => autoUnloadInterval;
        set
        {
            autoUnloadInterval = value;
            autoUnloadDelay.Cancel();
        }
    }

    IAssetCatalog IAssetFolder.Assets => this;

    public async Task<T> Load<T>(string path)
        where T : Object
        => await Resources.LoadAsync<T>(path).AsTask<T>() ?? throw new InvalidOperationException($"Asset of type {typeof(T).FullName} not found at path {path}");

    public void Unload(Object asset)
        => Resources.UnloadAsset(asset);

    public void UnloadUnused()
        => autoUnloadDelay.Cancel();

    private async void AutoUnload()
    {
        while (true)
        {
            await TaskUtils.TryDelay(AutoUnloadInterval ?? TimeSpan.MaxValue, autoUnloadDelay.Token);
            await Resources.UnloadUnusedAssets();
        }
    }

    public string GetPath(string relativePath)
        => relativePath;
}