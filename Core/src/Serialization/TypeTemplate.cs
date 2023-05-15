namespace Markwardt;

public record TypeTemplate(string Name, int ParameterCount)
{
    public static TypeTemplate FromText(string text)
    {
        string[] parts = text.Split('`', 2);
        return new TypeTemplate(parts[0], parts.Length == 2 ? int.Parse(parts[1]) : 0);
    }

    public static TypeTemplate FromType(Type type)
        => FromText(type.Name);

    public override string ToString()
        => ParameterCount == 0 ? Name : $"{Name}`{ParameterCount}";
}