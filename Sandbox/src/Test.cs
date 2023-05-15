namespace Markwardt;

public interface ITest<T>
{
    [Singleton(typeof(Test<>))]
    delegate ValueTask<ITest<T>> Factory<TId>(TId id);

    string Name { get; }
}

public class Test<T> : ITest<T>
{
    public static ValueTask<Test<T>> Construct<TId>(TId id)
        => ValueTask.FromResult(new Test<T>(id?.ToString() ?? "null"));

    private Test(string id)
    {
        this.id = id;
    }

    private readonly string id;

    public string Name => $"{id} ({typeof(T).Name})";

    public override string ToString()
        => Name;
}