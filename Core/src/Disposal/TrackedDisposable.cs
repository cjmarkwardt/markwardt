namespace Markwardt;

public interface ITrackedDisposable : IDisposable
{
    bool IsDisposed { get; }
}