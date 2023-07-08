namespace Markwardt;

[Singleton]
public interface ILooseAssetUnloader
{
    ValueTask Unload();
}

public class LooseAssetUnloader : ILooseAssetUnloader
{
    public async ValueTask Unload()
        => await Resources.UnloadUnusedAssets();
}