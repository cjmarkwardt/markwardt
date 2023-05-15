namespace Markwardt;

[Singleton<EnvironmentExiter>]
public interface IExiter
{
    bool IsExiting { get; }

    void Exit(object? info = null);
}