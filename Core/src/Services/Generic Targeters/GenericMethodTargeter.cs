namespace Markwardt;

public class GenericMethodTargeter : GenericTargeter
{
    public static IEnumerable<Type> GetTypeParameters(MethodBase method)
    {
        if (method is MethodInfo directMethod && directMethod.IsGenericMethodDefinition)
        {
            return directMethod.GetGenericArguments();
        }
        else
        {
            return Enumerable.Empty<Type>();
        }
    }

    public GenericMethodTargeter(MethodBase method)
        : base(method.Name, GetTypeParameters(method).Select(x => x.Name.ToLower()).ToList())
    {
        this.method = method;
    }

    private readonly MethodBase method;

    public MethodBase Close(IValueDictionary<string, Type> typeArguments)
    {
        if (method is MethodInfo directMethod && directMethod.IsGenericMethodDefinition)
        {
            return directMethod.MakeGenericMethod(directMethod.GetGenericArguments().Select(x => typeArguments[x.Name]).ToArray());
        }
        else
        {
            return method;
        }
    }

    public MethodBase Close(IDictionary<string, object?> arguments)
        => Close(GetTypeArguments(arguments));
}