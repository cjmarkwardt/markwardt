namespace Markwardt;

public interface IAssetFolder
{
    IAssetCatalog Assets { get; }
    
    string GetPath(string relativePath);
}

public static class AssetFolderUtils
{
    public static async Task<T> Load<T>(this IAssetFolder folder, string? path = null)
        where T : Object
        => await folder.Assets.Load<T>(folder.GetPath(path ?? typeof(T).Name));
    
    public static async Task<GameObject> Instantiate(this IAssetFolder folder, string path, Transform? parent = null)
    {
        GameObject prefab = await folder.Load<GameObject>(path);
        GameObject instance = parent != null ? GameObject.Instantiate(prefab.gameObject, parent) : GameObject.Instantiate(prefab.gameObject);
        instance.name = instance.name.Substring(0, instance.name.Length - "(Clone)".Length);
        return instance;
    }

    public static async Task<TPrefab> Instantiate<TPrefab>(this IAssetFolder folder, string path, Transform? parent = null)
        where TPrefab : notnull, Component
        => (await folder.Instantiate(path, parent)).GetComponent<TPrefab>() ?? throw new InvalidOperationException($"Component {typeof(TPrefab).FullName} not found in asset {path}");

    public static async Task<TPrefab> Instantiate<TPrefab>(this IAssetFolder folder, Transform? parent = null)
        where TPrefab : notnull, Component
        => await folder.Instantiate<TPrefab>(typeof(TPrefab).Name, parent);
}

public abstract record AssetFolder(IAssetFolder AssetParent) : IAssetFolder
{
    public IAssetCatalog Assets => AssetParent.Assets;

    protected abstract string Path { get; }

    public string GetPath(string relativePath)
        => AssetParent.GetPath($"{Path}/{relativePath}");
}