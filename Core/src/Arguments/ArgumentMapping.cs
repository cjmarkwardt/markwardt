namespace Markwardt;

public static class ArgumentMappingUtils
{
    public static IEnumerable<Argument<T>>? Map<T>(this IEnumerable<ArgumentMapping>? mappings, IEnumerable<Argument<T>>? arguments)
    {
        if (arguments == null)
        {
            return null;
        }
        else if (mappings == null)
        {
            return arguments;
        }

        IDictionary<string, string> mappedNames = mappings.ToDictionary(m => m.Argument, m => m.MappedArgument);
        List<Argument<T>> mappedArguments = new(arguments.Count());
        foreach (Argument<T> argument in arguments)
        {
            if (mappedNames.TryGetValue(argument.Name, out string? mappedName))
            {
                mappedArguments.Add(new Argument<T>(mappedName, argument.Value));
            }
            else
            {
                mappedArguments.Add(argument);
            }
        }

        return mappedArguments;
    }

    public static IEnumerable<Argument<T>>? Replace<T>(this IEnumerable<Argument<T>>? arguments, IEnumerable<Argument<T>>? replacements)
    {
        if (arguments == null)
        {
            return replacements;
        }
        else if (replacements == null)
        {
            return arguments;
        }
        else
        {
            Dictionary<string, Argument<T>> replaced = arguments.ToDictionary(a => a.Name);
            foreach (Argument<T> replacement in replacements)
            {
                replaced[replacement.Name] = replacement;
            }

            return replaced.Values;
        }
    }
}

public record ArgumentMapping(string Argument, string MappedArgument);