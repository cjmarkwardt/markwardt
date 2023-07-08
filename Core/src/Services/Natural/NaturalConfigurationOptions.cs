namespace Markwardt;

public record NaturalConfigurationOptions
{
    public Type? Implementation { get; init; } = null;
    public OpenServiceKind Kind { get; init; } = OpenServiceKind.Natural;
    public Type? Arguments { get; init; } = null;
}