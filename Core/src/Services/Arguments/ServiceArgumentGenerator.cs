namespace Markwardt;

public interface IServiceArgumentGenerator
{
    ValueTask Generate(IServiceResolver resolver, IDictionary<string, object?> arguments);
}

public static class ServiceArgumentGeneratorUtils
{
    public static async ValueTask<IDictionary<string, object?>?> Generate(this IServiceArgumentGenerator? generator, IServiceResolver resolver)
    {
        if (generator == null)
        {
            return null;
        }

        Dictionary<string, object?> arguments = new();
        await generator.Generate(resolver, arguments);
        return arguments;
    }
}

public class ServiceArgumentGenerator : IServiceArgumentGenerator
{
    public ServiceArgumentGenerator() { }

    public ServiceArgumentGenerator(IDictionary<string, object?> arguments)
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

    public async ValueTask Generate(IServiceResolver resolver, IDictionary<string, object?> arguments)
    {
        foreach (KeyValuePair<string, AsyncFunc<object?>> injector in injectors)
        {
            arguments[injector.Key] = await injector.Value();
        }
    }
}