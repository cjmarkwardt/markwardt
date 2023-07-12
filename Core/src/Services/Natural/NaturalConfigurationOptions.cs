namespace Markwardt;

public record NaturalConfigurationOptions
{
    public Type? Implementation { get; init; } = null;
    public OpenServiceKind Kind { get; init; } = OpenServiceKind.Natural;
    public Type? Arguments { get; init; } = null;

    public string? ConstructorId { get; init; } = null;
    public string? ConstructorName { get; init; } = null;
    public Type[]? ConstructorArguments { get; init; } = null;
}