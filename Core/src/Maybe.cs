namespace Markwardt;

public static class MaybeUtils
{
    public static IMaybe<T> AsMaybe<T>(this T obj)
        => new Maybe<T>(obj);

    public static IMaybe<T> AsNullableMaybe<T>(this T? obj)
        where T : class
        => Maybe.If<T>(obj != null, obj!);
}

public interface IMaybe<T>
{
    bool HasValue { get; }
    T Value { get; }

    bool TryGet(out T value)
    {
        if (HasValue)
        {
            value = Value;
            return true;
        }
        else
        {
            value = default!;
            return false;
        }
    }

    T Get(string? message = null)
        => HasValue ? Value : throw new InvalidOperationException(message ?? "Value is missing");

    T GetOr(T other)
        => HasValue ? Value : other;

    T? GetOrDefault()
        => HasValue ? Value : default;

    IMaybe<TConverted> Convert<TConverted>(Func<T, TConverted> convert)
        => HasValue ? new Maybe<TConverted>(convert(Value)) : default;

    async Task<IMaybe<TConverted>> Convert<TConverted>(Func<T, Task<TConverted>> convert)
        => HasValue ? new Maybe<TConverted>(await convert(Value)) : default;

    IMaybe<TCasted> Cast<TCasted>()
        => Convert(x => (TCasted)(object?)x!);
}

public static class Maybe
{
    public static IMaybe<T> Empty<T>()
        => default(Maybe<T>);

    public static IMaybe<T> If<T>(bool condition, Func<T> getTrueValue, Func<T>? getFalseValue = null)
        => condition ? getTrueValue() : getFalseValue != null ? getFalseValue() : default(Maybe<T>);

    public static IMaybe<T> If<T>(bool condition, T trueValue, T falseValue)
        => If(condition, () => trueValue, () => falseValue);

    public static IMaybe<T> If<T>(bool condition, T trueValue)
        => If(condition, () => trueValue);
}

public struct Maybe<T> : IMaybe<T>
{
    public static implicit operator Maybe<T>(T value)
        => new Maybe<T>(value);

    public Maybe()
    {
        HasValue = false;
        this.value = default!;
    }

    public Maybe(T value)
    {
        HasValue = true;
        this.value = value;
    }

    private readonly T value;

    public bool HasValue { get; }

    public T Value => HasValue ? value : throw new InvalidOperationException();
}