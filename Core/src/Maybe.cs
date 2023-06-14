namespace Markwardt;

public static class MaybeUtils
{
    public static Maybe<T> AsMaybe<T>(this T obj)
        => new Maybe<T>(obj);
}

public static class Maybe
{
    public static Maybe<T> If<T>(bool condition, Func<T> getTrueValue, Func<T>? getFalseValue = null)
        => condition ? getTrueValue() : getFalseValue != null ? getFalseValue() : default(Maybe<T>);

    public static Maybe<T> If<T>(bool condition, T trueValue, T falseValue)
        => If(condition, () => trueValue, () => falseValue);

    public static Maybe<T> If<T>(bool condition, T trueValue)
        => If(condition, () => trueValue);
}

public struct Maybe<T>
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

    public bool TryGet(out T value)
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

    public T GetOr(T other)
        => HasValue ? Value : other;

    public T? GetOrDefault()
        => HasValue ? Value : default;

    public Maybe<TConverted> Convert<TConverted>(Func<T, TConverted> convert)
        => HasValue ? new Maybe<TConverted>(convert(Value)) : default;

    public async Task<Maybe<TConverted>> Convert<TConverted>(Func<T, Task<TConverted>> convert)
        => HasValue ? new Maybe<TConverted>(await convert(Value)) : default;

    public Maybe<TCasted> Cast<TCasted>()
        => Convert(x => (TCasted)(object?)x!);
}