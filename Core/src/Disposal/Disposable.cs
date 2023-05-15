namespace Markwardt;

public class Disposable : IDisposable
{
    public Disposable(Action dispose)
    {
        this.dispose = dispose;
    }

    private readonly Action dispose;

    public void Dispose()
        => dispose();
}