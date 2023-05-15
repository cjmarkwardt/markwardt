namespace Markwardt;

public interface ITypeSerializer : IEnumerable<Type>
{
    void Register(Type type);

    string? TrySerialize(Type type);
    Type? TryDeserialize(string name);
}

public static class TypeSerializerUtils
{
    public static void RegisterAttribute(this ITypeSerializer serializer, Type attribute)
    {
        foreach (Type type in AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes()).Where(t => t.GetCustomAttribute(attribute) != null))
        {
            serializer.Register(type);
        }
    }

    public static void RegisterAttribute<TAttribute>(this ITypeSerializer serializer)
        where TAttribute : Attribute
        => serializer.RegisterAttribute(typeof(TAttribute));

    public static bool CanSerialize(this ITypeSerializer types, Type type)
        => types.TrySerialize(type) != null;

    public static bool CanDeserialize(this ITypeSerializer types, string name)
        => types.TryDeserialize(name) != null;
    
    public static string Serialize(this ITypeSerializer types, Type type)
        => types.TrySerialize(type) ?? throw new InvalidOperationException($"Type {type.FullName} not registered");

    public static Type Deserialize(this ITypeSerializer types, string name)
        => types.TryDeserialize(name) ?? throw new InvalidOperationException($"Type {name} not registered");
}

public class TypeSerializer : ITypeSerializer
{
    public TypeSerializer(bool includeBasics = true)
    {
        if (includeBasics)
        {
            Register(typeof(bool));
            Register(typeof(byte));
            Register(typeof(sbyte));
            Register(typeof(short));
            Register(typeof(ushort));
            Register(typeof(int));
            Register(typeof(uint));
            Register(typeof(long));
            Register(typeof(ulong));
            Register(typeof(decimal));
            Register(typeof(float));
            Register(typeof(double));
            Register(typeof(char));
            Register(typeof(string));
            Register(typeof(DateTime));
            Register(typeof(List<>));
            Register(typeof(HashSet<>));
            Register(typeof(Dictionary<,>));
        }
    }

    private readonly HashSet<Type> types = new();
    private readonly HashSet<string> names = new();

    private readonly Dictionary<string, Type> validatedNames = new();
    private readonly Dictionary<Type, string> validatedTypes = new();

    public void Register(Type type)
    {
        types.Add(type);
        names.Add(TypeName.FromType(type).Open().ToString());
    }

    public string? TrySerialize(Type type)
    {
        if (!validatedTypes.TryGetValue(type, out string? name))
        {
            name = CreateName(type);
            if (name == null)
            {
                return null;
            }

            validatedTypes.Add(type, name);
        }

        return name;
    }

    public Type? TryDeserialize(string name)
    {
        if (!validatedNames.TryGetValue(name, out Type? type))
        {
            type = CreateType(name);
            if (type == null)
            {
                return null;
            }

            validatedNames.Add(name, type);
        }

        return type;
    }

    public IEnumerator<Type> GetEnumerator()
        => types.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();

    private string? CreateName(Type type)
    {
        if (IsValid(type))
        {
            return TypeName.FromType(type).ToString();
        }

        return null;
    }

    private Type? CreateType(string name)
    {
        TypeName type = TypeName.FromText(name);
        if (IsValid(type))
        {
            return type.Find();
        }

        return null;
    }

    private bool IsValid(Type type)
    {
        if (type.IsGenericType)
        {
            if (!types.Contains(type.GetGenericTypeDefinition()) || type.GetGenericArguments().Any(a => IsValid(a)))
            {
                return false;
            }
        }
        else if (!types.Contains(type))
        {
            return false;
        }

        return true;
    }

    private bool IsValid(TypeName type)
    {
        if (type.IsOpen)
        {
            if (!names.Contains(type.Open().ToString()) || type.Arguments.Any(a => IsValid(a)))
            {
                return false;
            }
        }
        else if (!names.Contains(type.ToString()))
        {
            return false;
        }

        return true;
    }
}