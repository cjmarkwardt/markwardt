namespace Markwardt;

public record DelegatedArgumentGenerator(AsyncAction<IObjectContainer, IDictionary<string, object?>> Delegate) : IArgumentGenerator
{
    public DelegatedArgumentGenerator(AsyncAction<IDictionary<string, object?>> Delegate)
        : this(async (container, arguments) => await Delegate(arguments)) { }

    public async ValueTask Generate(IObjectContainer container, IDictionary<string, object?> arguments)
        => await Delegate(container, arguments);
}