namespace Markwardt;

public class Generalizer
{
    public Generalizer(string targetName, IEnumerable<string> typeParameters)
    {
        this.targetName = targetName;
        this.typeParameters = typeParameters;   
    }

    private readonly string targetName;
    private readonly IEnumerable<string> typeParameters;

    public IValueDictionary<string, Type> GetTypeArguments(IDictionary<string, object?>? arguments)
    {
        if (!typeParameters.Any())
        {
            return ValueDictionary<string, Type>.Empty;
        }

        Dictionary<string, Type> typeArguments = new();
        foreach (string typeParameter in typeParameters)
        {
            if (arguments != null && arguments.TryGetValue(typeParameter.ToLower(), out object? argument))
            {
                if (argument is Type type)
                {
                    typeArguments.Add(typeParameter.ToLower(), type);
                }
                else
                {
                    throw new InvalidOperationException($"Argument given for type parameter {typeParameter} was not a type in generic target {targetName}");
                }
            }
            else
            {
                throw new InvalidOperationException($"No type argument given for type parameter {typeParameter} in generic target {targetName}");
            }
        }

        return typeArguments.ToValueDictionary();
    }
}