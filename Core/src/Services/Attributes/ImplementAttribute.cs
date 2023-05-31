namespace Markwardt;

public class ImplementAttribute : Attribute
{
    public ImplementAttribute(Type target, Type? arguments = null)
    {
        Target = target;
        Arguments = arguments;
    }

    public Type Target { get; }
    public Type? Arguments { get; }
}

public class ImplementAttribute<TTarget> : ImplementAttribute
    where TTarget : class
{
    public ImplementAttribute()
        : base(typeof(TTarget)) { }
}

public class ImplementAttribute<TTarget, TArguments> : ImplementAttribute
    where TTarget : class
    where TArguments : IArgumentGenerator
{
    public ImplementAttribute()
        : base(typeof(TTarget), typeof(TArguments)) { }
}