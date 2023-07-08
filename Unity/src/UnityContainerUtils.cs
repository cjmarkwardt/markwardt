namespace Markwardt;

public static class UnityContainerUtils
{
    public static void ConfigureAsset<T>(this IServiceContainer container, Path path, ServiceKind kind = ServiceKind.Singleton)
        where T : notnull, Object
        => container.ConfigureSource<T, IAssetLoader>(async loader => await loader.Load<T>(path), kind);
    
    public static void ConfigureAsset<T, TConfiguration>(this IServiceContainer container, Path path, ServiceKind kind = ServiceKind.Singleton)
        where T : notnull, Object
        where TConfiguration : IServiceConfiguration, new()
        => container.ConfigureSpecificSource<T, TConfiguration, IAssetLoader>(async loader => await loader.Load<T>(path), kind);

    public static void ConfigurePrefab<T>(this IServiceContainer container, Path path, ServiceKind kind = ServiceKind.Transient)
        where T : notnull, Component
        => container.ConfigureSource<T, IAssetLoader>(async loader => await loader.Instantiate<T>(path), kind);
    
    public static void ConfigurePrefab<T, TConfiguration>(this IServiceContainer container, Path path, ServiceKind kind = ServiceKind.Transient)
        where T : notnull, Component
        where TConfiguration : IServiceConfiguration, new()
        => container.ConfigureSpecificSource<T, TConfiguration, IAssetLoader>(async loader => await loader.Instantiate<T>(path), kind);
}