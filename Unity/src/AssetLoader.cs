namespace Markwardt;

[Singleton]
public interface IAssetLoader
{
    ValueTask<Failable<T>> TryLoad<T>(Path path);
    void Unload(object asset);

    async ValueTask<T> Load<T>(Path path)
        => (await TryLoad<T>(path)).Result;

    async Task<GameObject> Instantiate(Path path, Transform? parent = null)
    {
        GameObject prefab = await Load<GameObject>(path);
        GameObject instance = parent != null ? GameObject.Instantiate(prefab.gameObject, parent) : GameObject.Instantiate(prefab.gameObject);
        instance.name = instance.name.Substring(0, instance.name.Length - "(Clone)".Length);
        return instance;
    }

    async Task<TPrefab> Instantiate<TPrefab>(Path path, Transform? parent = null)
        where TPrefab : notnull, Component
        => (await Instantiate(path, parent)).GetComponent<TPrefab>() ?? throw new InvalidOperationException($"Component {typeof(TPrefab).FullName} not found in asset {path}");
}

public class AssetLoader : IAssetLoader
{
    public async ValueTask<Failable<T>> TryLoad<T>(Path path)
    {
        if (!typeof(Object).IsAssignableFrom(typeof(T)))
        {
            return new Failure($"Asset of type {typeof(T).FullName} must be derived from Object at path {path}");
        }

        Object? asset = await Resources.LoadAsync(path.ToString("/"), typeof(T)).AsTask();
        if (asset == null)
        {
            return new Failure($"Asset of type {typeof(T).FullName} not found at path {path}");
        }

        return (T)(object)asset;
    }

    public void Unload(object asset)
    {
        if (asset is Object obj)
        {
            Resources.UnloadAsset(obj);
        }
    }
}