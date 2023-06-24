namespace Markwardt;

public interface IMutableValue : IValue
{
    void Set(object? value);
}

public interface IMutableValue<T> : IValue<T>
{
    new IMutableValue Generalized { get; }

    void Set(T value);
}