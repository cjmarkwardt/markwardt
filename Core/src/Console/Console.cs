namespace Markwardt;

[RoutedSingleton<ISystemConsole>]
public interface IConsole : IConsoleWriter, IConsoleReader { }