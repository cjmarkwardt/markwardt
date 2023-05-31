namespace Markwardt;

public class FixedArgumentGenerator : IArgumentGenerator
{
    public FixedArgumentGenerator(IDictionary<string, object?> arguments)
    {
        this.arguments = arguments;
    }

    private readonly IDictionary<string, object?> arguments;

    public ValueTask Generate(IObjectContainer container, IDictionary<string, object?> arguments)
    {
        foreach (KeyValuePair<string, object?> argument in this.arguments)
        {
            arguments[argument.Key] = argument.Value;
        }

        return new ValueTask();
    }
}