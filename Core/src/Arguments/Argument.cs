namespace Markwardt;

public class Argument<T> : IEquatable<Argument<T>>
{
    public Argument(string name, T value)
    {
        Name = name.ToLower();
        Value = value;
    }

    public string Name { get; }
    public T Value { get; }

    public override bool Equals(object? obj)
        => Equals(obj as Argument<T>);

    public bool Equals(Argument<T>? other)
        => other != null && Name == other.Name && Value.NullableEquals(other.Value);

    public override int GetHashCode()
        => HashCode.Combine(Name, Value);
}

public interface IArgument<T>
{
    string Name { get; }

    
}