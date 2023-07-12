namespace Markwardt;

public class Claim : IDisposable
{
    public Claim(object instance, Action dispose, Func<bool>? isDisposed = null)
    {
        this.instance = instance;
        this.dispose = dispose;
        this.isDisposed = isDisposed;
    }

    private readonly object instance;
    private readonly Action dispose;
    private readonly Func<bool>? isDisposed;

    private bool internalIsDisposed;

    public bool IsDisposed => isDisposed != null ? isDisposed() : internalIsDisposed;

    public object Instance => !IsDisposed ? instance : throw new ObjectDisposedException(GetType().FullName);

    public void Dispose()
    {
        if (!IsDisposed)
        {
            internalIsDisposed = true;
            dispose();
        }
    }

    public Claim Convert(Func<object, object> convert)
        => new Claim(convert(Instance), () => Dispose(), () => IsDisposed);

    public Claim<T> Cast<T>()
        where T : notnull
        => new Claim<T>((T)Instance, () => Dispose(), () => IsDisposed);
}

public class Claim<T> : Claim
    where T : notnull
{
    public static implicit operator T(Claim<T> claim)
        => claim.Instance;

    public Claim(T instance, Action dispose, Func<bool>? isDisposed = null)
        : base(instance, dispose, isDisposed) { }

    public new T Instance => (T)base.Instance;

    public Claim<TConverted> Convert<TConverted>(Func<T, TConverted> convert)
        where TConverted : notnull
        => new Claim<TConverted>(convert(Instance), () => Dispose(), () => IsDisposed);
}