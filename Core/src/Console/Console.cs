namespace Markwardt;

[RoutedService<ISystemConsole>]
public interface IConsole : IConsoleWriter, IConsoleReader { }