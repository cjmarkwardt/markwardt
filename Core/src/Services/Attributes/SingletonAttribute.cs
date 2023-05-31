namespace Markwardt;

public class SingletonAttribute : Attribute
{
    public SingletonAttribute(Type? arguments = null)
    {
        Arguments = arguments;
    }

    public Type? Arguments { get; }
}

public class SingletonAttribute<TArguments> : SingletonAttribute
    where TArguments : IArgumentGenerator
{
    public SingletonAttribute()
        : base(typeof(TArguments)) { }
}