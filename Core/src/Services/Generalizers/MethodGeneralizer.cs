namespace Markwardt;

public class MethodGeneralizer : Generalizer
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

    public MethodGeneralizer(MethodBase method)
        : base(method.Name, GetTypeParameters(method).Select(x => x.Name.ToLower()).ToList())
    {
        this.method = method;
    }

    private readonly MethodBase method;

    public MethodBase Specify(IValueDictionary<string, Type> typeArguments)
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
        => Specify(GetTypeArguments(arguments));
}