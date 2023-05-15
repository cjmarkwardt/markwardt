namespace Markwardt;

public interface IApplicationArguments
{
    IReadOnlyList<string> Value { get; }
}

public record ApplicationArguments(IReadOnlyList<string> Value) : IApplicationArguments;