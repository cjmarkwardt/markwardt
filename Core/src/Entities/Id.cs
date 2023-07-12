namespace Markwardt;

public record struct Id<T>(string Value)
{
    public static implicit operator string(Id<T> id)
        => id.Value;

    public static implicit operator Id<T>(string value)
        => new Id<T>(value);

    public Id<TCasted> Cast<TCasted>()
        => new Id<TCasted>(Value);
}