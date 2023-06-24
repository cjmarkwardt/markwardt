namespace Markwardt;

public interface IValue
{
    object? Get();

    IObservable<object?> Observe();
}

public interface IValue<out T> : IObservable<T>
{
    IValue Generalized { get; }

    T Get();
}

public static class Value
{
    public static IMutableValue<T> Create<T>(T value)
        => new StoredValue<T>(value);

    public static IValue<T> Create<T>(Func<T> getValue, IObservable<T> changes)
        => new DerivedValue<T>(getValue, changes);

    public static IMutableValue<T> Create<T>(Func<T> getValue, Action<T> setValue, IObservable<T> changes)
        => new DerivedMutableValue<T>(getValue, setValue, changes);

    public static IValue<TSelected> Select<T, TSelected>(this IValue<T> value, Func<T, TSelected> select)
        => Create(() => select(value.Get()), value.Select(x => select(x)).Skip(1));

    public static IMutableValue<TSelected> Select<T, TSelected>(this IMutableValue<T> value, Func<T, TSelected> select, Func<TSelected, T> selectBack)
        => Create(() => select(value.Get()), x => value.Set(selectBack(x)), value.Select(x => select(x)).Skip(1));

    public static IValue<TCombined> Combine<T1, T2, TCombined>(this IValue<T1> value1, IValue<T2> value2, Func<T1, T2, TCombined> combine)
        => Create(() => combine(value1.Get(), value2.Get()), value1.CombineLatest(value2, (v1, v2) => combine(v1, v2)).Skip(1));

    public static IValue<TCombined> Combine<T1, T2, T3, TCombined>(this IValue<T1> value1, IValue<T2> value2, IValue<T3> value3, Func<T1, T2, T3, TCombined> combine)
        => Create(() => combine(value1.Get(), value2.Get(), value3.Get()), value1.CombineLatest(value2, value3, (v1, v2, v3) => combine(v1, v2, v3)).Skip(1));
    
    public static IValue<TCombined> Combine<T1, T2, T3, T4, TCombined>(this IValue<T1> value1, IValue<T2> value2, IValue<T3> value3, IValue<T4> value4, Func<T1, T2, T3, T4, TCombined> combine)
        => Create(() => combine(value1.Get(), value2.Get(), value3.Get(), value4.Get()), value1.CombineLatest(value2, value3, value4, (v1, v2, v3, v4) => combine(v1, v2, v3, v4)).Skip(1));
    
    public static IValue<TCombined> Combine<T1, T2, T3, T4, T5, TCombined>(this IValue<T1> value1, IValue<T2> value2, IValue<T3> value3, IValue<T4> value4, IValue<T5> value5, Func<T1, T2, T3, T4, T5, TCombined> combine)
        => Create(() => combine(value1.Get(), value2.Get(), value3.Get(), value4.Get(), value5.Get()), value1.CombineLatest(value2, value3, value4, value5, (v1, v2, v3, v4, v5) => combine(v1, v2, v3, v4, v5)).Skip(1));
}