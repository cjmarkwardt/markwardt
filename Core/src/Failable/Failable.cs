namespace Markwardt;

public static class FailableUtils
{
    public static Failable<T> AsFailable<T>(this T result)
        => Failable.Success(result);
}

public class Failable
{
    private static readonly Failable success = new Failable(null);

    public static implicit operator Task<Failable>(Failable failable)
        => Task.FromResult<Failable>(failable);

    public static implicit operator ValueTask<Failable>(Failable failable)
        => new ValueTask<Failable>(failable);

    public static implicit operator Failable(Failure failure)
        => new Failable(failure);

    public static Failable Success()
        => success;

    public static Failable Fail(Failure failure)
        => new Failable(failure);

    public static Failable<T> Success<T>(T result)
        => new Failable<T>(result);

    public static Failable<T> Fail<T>(Failure failure)
        => new Failable<T>(failure);

    internal Failable(Failure? failure)
    {
        Failure = failure;
    }

    public Failure? Failure { get; }

    public Exception? TryAsException()
        => Failure == null ? null : Failure.AsException();

    public bool TryAsException([NotNullWhen(true)] out Exception? exception)
    {
        exception = TryAsException();
        return exception != null;
    }

    public void Verify()
    {
        if (Failure != null)
        {
            throw Failure.AsException();
        }
    }

    public void Deconstruct(out bool isFailed, out Failure failure)
    {
        isFailed = Failure != null;
        failure = Failure!;
    }

    public override string ToString()
        => Failure != null ? $"Failure ({Failure.Message})" : "Success";
}

public class Failable<T> : Failable
{
    public static implicit operator Task<Failable<T>>(Failable<T> failable)
        => Task.FromResult<Failable<T>>(failable);
        
    public static implicit operator ValueTask<Failable<T>>(Failable<T> failable)
        => new ValueTask<Failable<T>>(failable);

    public static implicit operator Failable<T>(T value)
        => new Failable<T>(value);

    public static implicit operator Failable<T>(Failure failure)
        => new Failable<T>(failure);

    internal Failable(T result)
        : base(null)
    {
        this.result = result;
    }

    internal Failable(Failure failure)
        : base(failure) { }

    private T? result;
    public T Result => result ?? throw Failure!.AsException();

    public void Deconstruct(out bool isFailed, out Failure failure, out T result)
    {
        isFailed = Failure != null;
        failure = Failure!;
        result = this.result!;
    }

    public Failable<TConverted> Convert<TConverted>(Func<T, Failable<TConverted>> convert)
    {
        if (Failure != null)
        {
            return Failure;
        }

        Failable<TConverted> tryConvert = convert(Result);
        if (tryConvert.Failure != null)
        {
            return tryConvert.Failure;
        }

        return tryConvert.Result;
    }

    public Failable<TConverted> Convert<TConverted>(Func<T, TConverted> convert)
        => Convert<TConverted>(r => Failable.Success(convert(r)));

    public async Task<Failable<TConverted>> Convert<TConverted>(Func<T, Task<Failable<TConverted>>> convert)
    {
        if (Failure != null)
        {
            return Failure;
        }

        Failable<TConverted> tryConvert = await convert(Result);
        if (tryConvert.Failure != null)
        {
            return tryConvert.Failure;
        }

        return tryConvert.Result;
    }

    public async Task<Failable<TConverted>> Convert<TConverted>(Func<T, Task<TConverted>> convert)
        => await Convert<TConverted>(async r => Failable.Success(await convert(r)));

    public Failable<TCasted> Cast<TCasted>()
        => Convert(r => (TCasted)(object?)r!);

    public override string ToString()
        => Failure != null ? $"Failure ({Failure.Message})" : $"Success ({Result})";
}