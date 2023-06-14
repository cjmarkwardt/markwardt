namespace Markwardt;

public interface IObjectContainer : IObjectResolver, IMultiDisposable
{
    bool CanOverride(ObjectTag tag);
    void Override(ObjectTag tag, IObjectScheme newScheme);
}

public static class ObjectContainerUtils
{
    public static bool CanOverride<T, TScheme>(this IObjectContainer container)
        where TScheme : IObjectScheme<T>
        => container.CanOverride(new ObjectTag(typeof(T), typeof(TScheme)));
        
    public static bool CanOverride<T>(this IObjectContainer container)
        => container.CanOverride(typeof(T));

    public static void Override<T, TScheme>(this IObjectContainer container, IObjectScheme<T> newScheme)
        where TScheme : IObjectScheme<T>
        => container.Override(new ObjectTag(typeof(T), typeof(TScheme)), newScheme);

    public static void Override<T>(this IObjectContainer container, IObjectScheme<T> newScheme)
        => container.Override(typeof(T), newScheme);
}

public class ObjectContainer : ManagedAsyncDisposable, IObjectContainer
{
    public ObjectContainer(IObjectSchemeGenerator? schemeGenerator = null)
    {
        this.schemeGenerator = schemeGenerator ?? new DefaultSchemeGenerator();
    }
    
    private readonly IObjectSchemeGenerator schemeGenerator;
    private readonly Dictionary<ObjectTag, Entry> entries = new();

    public bool CanOverride(ObjectTag tag)
        => !entries.TryGetValue(tag, out Entry? entry) || !entry.IsSingletonCreationStarted;

    public void Override(ObjectTag tag, IObjectScheme newScheme)
    {
        if (!CanOverride(tag))
        {
            throw new InvalidOperationException();
        }

        SetEntry(tag, newScheme);
    }

    public async ValueTask<Maybe<object?>> Create(ObjectTag tag, Maybe<IObjectArgumentGenerator> arguments = default)
        => GetEntry(tag).TryGet(out Entry? entry) && entry.CanBuild ? await entry.Build(arguments) : default;

    public async ValueTask<Maybe<object?>> Resolve(ObjectTag tag)
        => GetEntry(tag).TryGet(out Entry? entry) && entry.IsSingleton ? await entry.GetSingleton() : Create(tag);

    private Entry SetEntry(ObjectTag tag, IObjectScheme scheme)
    {
        if (entries.TryGetValue(tag, out Entry? entry))
        {
            Disposal.Remove(entry);
        }

        entry = new Entry(this, tag, scheme);
        Disposal.Add(entry);
        entries[tag] = entry;
        return entry;
    }

    private Maybe<Entry> GetEntry(ObjectTag tag)
    {
        if (entries.TryGetValue(tag, out Entry? entry))
        {
            return entry;
        }
        else if (schemeGenerator.Generate(tag).TryGet(out IObjectScheme? generatedScheme))
        {
            return SetEntry(tag, generatedScheme);
        }
        else
        {
            return default;
        }
    }

    private class Entry : ManagedAsyncDisposable
    {
        public Entry(IObjectResolver resolver, ObjectTag tag, IObjectScheme scheme)
        {
            this.resolver = resolver;

            builder = scheme.GetBuilder(tag);

            if (scheme.GetSingletonBuilder(tag).TryGet(out IObjectBuilder? singletonBuilder))
            {
                singleton = new AsyncLazy<object?>(async () => await singletonBuilder.Build(resolver));
            }

            Disposal.Add(singleton);
        }

        private readonly IObjectResolver resolver;
        private readonly Maybe<IObjectBuilder> builder;
        private readonly Maybe<AsyncLazy<object?>> singleton;

        public bool IsSingleton => singleton.HasValue;
        public bool IsSingletonCreationStarted => singleton.HasValue && singleton.Value.IsValueCreationStarted;

        public bool CanBuild => builder.HasValue;

        public async ValueTask<object?> GetSingleton()
            => await singleton.Value.GetValue();

        public async ValueTask<object?> Build(Maybe<IObjectArgumentGenerator> arguments)
            => await builder.Value.Build(resolver, arguments);
    }
}