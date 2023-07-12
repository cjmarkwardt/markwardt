namespace Markwardt;

[SubstituteAs<ISystemConsole>]
public interface IConsole : IConsoleWriter, IConsoleReader { }