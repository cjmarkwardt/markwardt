namespace Markwardt;

[Singleton<CanvasManager>]
public interface ICanvasManager
{
    RectTransform GetLayerContainer<TLayer>(TLayer layer)
        where TLayer : Enum;
}

public record CanvasManager(Canvas Canvas, IAssetCatalogOld Assets) : ICanvasManager
{
    public RectTransform GetLayerContainer<TLayer>(TLayer layer)
        where TLayer : Enum
    {
        int GetValue(TLayer layer)
            => (int)(object)layer;

        RectTransform? container = FindLayerContainer(GetValue(layer));
        if (container == null)
        {
            container = new GameObject($"{GetValue(layer)} {layer}").AddComponent<RectTransform>();
            container.SetParent(Canvas.transform);
            container.anchorMin = Vector2.zero;
            container.anchorMax = Vector2.one;
            container.anchoredPosition = container.sizeDelta = Vector3.zero;

            foreach (TLayer previousLayer in EnumUtils.GetValues<TLayer>().OrderBy(l => GetValue(l)).TakeWhile(l => GetValue(l) != GetValue(layer)).Reverse())
            {
                RectTransform? previousContainer = FindLayerContainer(GetValue(previousLayer));
                if (previousContainer != null)
                {
                    container.SetSiblingIndex(previousContainer.GetSiblingIndex());
                    break;
                }
            }
        }

        return container;
    }

    private RectTransform? FindLayerContainer(int layer)
    {
        foreach (RectTransform container in Canvas.transform.OfType<RectTransform>())
        {
            if (container.gameObject.name.StartsWith(layer.ToString()))
            {
                return container;
            }
        }

        return null;
    }
}