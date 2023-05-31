namespace Markwardt;

public interface IArgumentGenerator
{
    ValueTask Generate(IObjectContainer container, IDictionary<string, object?> arguments);
}

public static class ArgumentGeneratorUtils
{
    public static async ValueTask<IDictionary<string, object?>> Generate(this IArgumentGenerator generator, IObjectContainer container)
    {
        Dictionary<string, object?> arguments = new();
        await generator.Generate(container, arguments);
        return arguments;
    }
}

public abstract class ArgumentGenerator : IArgumentGenerator
{
    public static IArgumentGenerator Create(IDictionary<string, object?> arguments)
        => new FixedArgumentGenerator(arguments);

    private Dictionary<string, AsyncFunc<object?>> injectors = new();

    protected void Set(string name, AsyncFunc<object?> getValue)
        => injectors[name] = getValue;

    protected void Set(string name, Func<object?> getValue)
        => Set(name, () => Task.FromResult(getValue()));

    protected void Clear(string name)
        => injectors.Remove(name);

    public async ValueTask Generate(IObjectContainer container, IDictionary<string, object?> arguments)
    {
        foreach (KeyValuePair<string, AsyncFunc<object?>> injector in injectors)
        {
            arguments[injector.Key] = await injector.Value();
        }
    }
}