namespace Markwardt;

public class ConstructLocator : IMethodLocator
{
    public MethodBase? Locate(Type type)
    {
        bool IsCopyConstructor(ConstructorInfo constructor)
            => constructor.GetParameters().Length == 1 && constructor.GetParameters().First().ParameterType == type;

        IEnumerable<MethodBase> methods = type.GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).Where(c => !IsCopyConstructor(c)).Cast<MethodBase>().Concat(type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic));

        MethodBase? attributed = methods.FirstOrDefault(m => m.GetCustomAttribute<FactoryAttribute>() != null);
        if (attributed != null)
        {
            return attributed;
        }

        MethodBase? staticCreate = methods.OfType<MethodInfo>().Where(m => m.Name == "Construct").OrderByDescending(m => m.GetParameters().Length).FirstOrDefault();
        if (staticCreate != null)
        {
            return staticCreate;
        }

        MethodBase? longestConstructor = methods.OfType<ConstructorInfo>().OrderByDescending(m => m.GetParameters().Length).FirstOrDefault();
        if (longestConstructor != null)
        {
            return longestConstructor;
        }

        return null;
    }
}