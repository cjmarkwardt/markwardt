namespace Markwardt;

public interface IIdentifiable
{
    public string Id { get; }
}

public static class IdentifiableUtils
{
    public static Id<T> AsId<T>(this T target)
        where T : IIdentifiable
        => new Id<T>(target.Id);
}

public class BaseIdentifiable : IIdentifiable
{
    public BaseIdentifiable(string id)
    {
        Id = id;
    }

    public string Id { get; }
}