namespace Markwardt;

[AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
public class MapTypeArgumentAttribute : Attribute
{
    public MapTypeArgumentAttribute(string argument, string mappedArgument)
    {
        Mapping = new ArgumentMapping(argument, mappedArgument);
    }

    public ArgumentMapping Mapping { get; }
}