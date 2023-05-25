namespace Markwardt;

public interface IOptional<out T>
{
    bool HasValue { get; }
    T Value { get; }
}

public class Optional<T> : IOptional<T>
{
    public static implicit operator Optional<T>(T value)
        => new Optional<T>(value);

    public static implicit operator Task<Optional<T>>(Optional<T> value)
        => Task.FromResult(value);

    public static implicit operator ValueTask<Optional<T>>(Optional<T> value)
        => new ValueTask<Optional<T>>(value);

    public static Optional<T> Missing { get; } = new Optional<T>();

    private Optional()
    {
        value = default!;
        HasValue = false;
    }

    public Optional(T value)
    {
        this.value = value;
        HasValue = true;
    }

    private readonly T value;

    public bool HasValue { get; }

    public T Value => HasValue ? value : throw new InvalidOperationException("Value is missing");
}