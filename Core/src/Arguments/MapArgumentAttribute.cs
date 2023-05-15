namespace Markwardt;

[AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
public class MapArgumentAttribute : Attribute
{
    public MapArgumentAttribute(string argument, string mappedArgument)
    {
        Mapping = new ArgumentMapping(argument, mappedArgument);
    }

    public ArgumentMapping Mapping { get; }
}