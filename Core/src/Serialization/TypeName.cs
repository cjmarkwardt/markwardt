namespace Markwardt;

public class TypeName
{
    public static TypeName FromText(string text)
    {
        string[] majorParts = text.Split(':', 2);

        string type = majorParts[0];
        string[] typeParts = type.Split('/', 2);
        string? @namespace = typeParts[0].Length == 0 ? null : typeParts[0];
        List<TypeTemplate> templates = typeParts[1].Split('+').Select(t => TypeTemplate.FromText(t)).ToList();

        List<TypeName> arguments = new();
        string rawArguments = majorParts[1];
        StringBuilder argument = new();
        int level = 0;
        foreach (char character in rawArguments)
        {
            if (character == '(')
            {
                if (level != 0)
                {
                    argument.Append(character);
                }

                level++;
            }
            else if (character == ')')
            {
                level--;

                if (level == 0)
                {
                    arguments.Add(FromText(argument.ToString()));
                    argument.Clear();
                }
                else
                {
                    argument.Append(character);
                }
            }
            else
            {
                argument.Append(character);
            }
        }

        if (templates.Sum(t => t.ParameterCount) == 0 || !arguments.Any())
        {
            return new TypeName(@namespace, templates);
        }
        else
        {
            return new TypeName(@namespace, templates, arguments);
        }
    }

    public static TypeName FromType(Type type)
    {
        IList<TypeTemplate> templates = new List<TypeTemplate>();
        Type? current = type;
        while (current != null)
        {
            templates.Add(TypeTemplate.FromType(current));
            current = current.DeclaringType;
        }

        templates = templates.Reverse().ToList();

        if (type.IsGenericTypeDefinition)
        {
            return new TypeName(type.Namespace, templates);
        }
        else
        {
            return new TypeName(type.Namespace, templates, type.GetGenericArguments().Select(t => FromType(t)));
        }
    }

    public TypeName(string? @namespace, IEnumerable<TypeTemplate> templates)
    {
        Namespace = @namespace;
        Templates = templates;
        Arguments = Enumerable.Empty<TypeName>();
    }

    public TypeName(string? @namespace, IEnumerable<TypeTemplate> templates, IEnumerable<TypeName> arguments)
    {
        Namespace = @namespace;
        Templates = templates;
        Arguments = arguments;

        if (Arguments.Count() != ParameterCount)
        {
            throw new InvalidOperationException();
        }
    }

    public string? Namespace { get; }
    public IEnumerable<TypeTemplate> Templates { get; }
    public IEnumerable<TypeName> Arguments { get; }

    public int ParameterCount => Templates.Sum(t => t.ParameterCount);

    public bool IsClosed => Arguments.Count() == ParameterCount;
    public bool IsOpen => !IsClosed;

    private string SystemNameHeader => $"{(Namespace == null ? string.Empty : $"{Namespace}.")}{string.Join("+", Templates)}";
    public string SystemName => IsClosed ? SystemNameHeader : $"{SystemNameHeader}[{string.Join(", ", Arguments.Select(p => p.SystemName))}]";

    public TypeName Open()
    {
        if (IsOpen || ParameterCount == 0)
        {
            return this;
        }

        return new TypeName(Namespace, Templates);
    }

    public TypeName Close(IEnumerable<TypeName> arguments)
        => new TypeName(Namespace, Templates, arguments);

    public Type Find()
    {
        Type? type = Type.GetType(SystemName);
        if (type == null)
        {
            type = AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes()).FirstOrDefault(t => t.FullName == SystemName);
        }

        return type ?? throw new InvalidOperationException($"Type {SystemName} not found");
    }

    public override string ToString()
        => $"{Namespace}/{string.Join('+', Templates)}:{string.Concat(Arguments.Select(a => $"({a})"))}";

    public override bool Equals(object obj)
        => obj is TypeName name && Namespace == name.Namespace && Templates.SequenceEqual(name.Templates) && Arguments.SequenceEqual(name.Arguments);

    public override int GetHashCode()
    {
        HashCode hash = new();

        hash.Add(Namespace);

        foreach (TypeTemplate template in Templates)
        {
            hash.Add(template);
        }

        foreach (TypeName argument in Arguments)
        {
            hash.Add(argument);
        }

        return hash.ToHashCode();
    }
}