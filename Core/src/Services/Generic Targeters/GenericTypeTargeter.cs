namespace Markwardt;

public class GenericTypeTargeter : GenericTargeter
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

    public GenericTypeTargeter(Type type)
        : base(type.Name, GetTypeParameters(type).Select(x => x.Name.ToLower()).ToList())
    {
        this.type = type;
    }

    private readonly Type type;

    public Type Close(IValueDictionary<string, Type> typeArguments)
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
        => Close(GetTypeArguments(arguments));
}