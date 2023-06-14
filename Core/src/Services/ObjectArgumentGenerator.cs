namespace Markwardt;

public interface IObjectArgumentGenerator
{
    ValueTask Generate(IObjectResolver resolver, IDictionary<string, object?> arguments);
}

public static class ObjectArgumentGeneratorUtils
{
    public static async ValueTask<IDictionary<string, object?>> Generate(this IObjectArgumentGenerator generator, IObjectResolver resolver)
    {
        Dictionary<string, object?> arguments = new();
        await generator.Generate(resolver, arguments);
        return arguments;
    }

    public static async ValueTask<Maybe<IDictionary<string, object?>>> Generate(this Maybe<IObjectArgumentGenerator> generator, IObjectResolver resolver)
        => await generator.Convert(async x => await x.Generate(resolver));
}

public class ObjectArgumentGenerator : IObjectArgumentGenerator
{
    public ObjectArgumentGenerator() { }

    public ObjectArgumentGenerator(IDictionary<string, object?> arguments)
    {
        foreach (KeyValuePair<string, object?> argument in arguments)
        {
            Set(argument.Key, argument.Value);
        }
    }

    private Dictionary<string, AsyncFunc<object?>> injectors = new();

    protected void Set(string name, AsyncFunc<object?> getValue)
        => injectors[name] = getValue;

    protected void Set(string name, Func<object?> getValue)
        => Set(name, () => Task.FromResult(getValue()));

    protected void Set(string name, object? value)
        => Set(name, () => Task.FromResult(value));

    protected void Clear(string name)
        => injectors.Remove(name);

    public async ValueTask Generate(IObjectResolver container, IDictionary<string, object?> arguments)
    {
        foreach (KeyValuePair<string, AsyncFunc<object?>> injector in injectors)
        {
            arguments[injector.Key] = await injector.Value();
        }
    }
}