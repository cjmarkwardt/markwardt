namespace Markwardt;

public class TypeGeneralizer : Generalizer
{
    public static IEnumerable<Type> GetTypeParameters(Type type)
    {
        if (type.IsGenericTypeDefinition)
        {
            return type.GetGenericArguments();
        }
        else
        {
            return Enumerable.Empty<Type>();
        }
    }

    public TypeGeneralizer(Type type)
        : base(type.Name, GetTypeParameters(type).Select(x => x.Name.ToLower()).ToList())
    {
        this.type = type;
    }

    private readonly Type type;

    public Type Specify(IValueDictionary<string, Type> typeArguments)
    {
        if (type.IsGenericTypeDefinition)
        {
            return type.MakeGenericType(type.GetGenericArguments().Select(x => typeArguments[x.Name]).ToArray());
        }
        else
        {
            return type;
        }
    }

    public Type Close(IDictionary<string, object?> arguments)
        => Specify(GetTypeArguments(arguments));
}