namespace Markwardt;

[RoutedService<IConsole>]
public interface IConsoleWriter
{
    IObservable<(object Target, bool IsError)> Writes { get; }

    void Write(object target, bool isError = false);
}

public abstract class ConsoleWriter : IConsoleWriter
{
    private readonly Subject<(object, bool)> writes = new();
    public IObservable<(object Target, bool IsError)> Writes => writes;

    public void Write(object target, bool isError = false)
    {
        WriteTarget(target, isError);
        writes.OnNext((target, isError));
    }

    protected abstract void WriteTarget(object target, bool isError);
}