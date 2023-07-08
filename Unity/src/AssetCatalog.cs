namespace Markwardt;

public interface IAssetCatalog : IAssetFolder
{
    Task<T> Load<T>(string path)
        where T : Object;

    void Unload(Object asset);

    void UnloadUnused();
}