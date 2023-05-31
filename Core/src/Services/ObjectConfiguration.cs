namespace Markwardt;

public interface IObjectConfiguration : IObjectContainer
{
    IObjectProfile? GetProfile(Type target);

    void Configure(IObjectProfile profile);
}

public static class ObjectConfigurationUtils
{
    public static void Configure(this IObjectConfiguration configuration, Type target, IObjectBuilder? builder, IObjectBuilder? singletonBuilder)
        => configuration.Configure(new ObjectProfile(target, builder, singletonBuilder));

    public static void Configure(this IObjectConfiguration configuration, Type target)
        => configuration.Configure(ObjectProfile.Scan(target).NotNull());
}

public class ObjectConfiguration : ManagedAsyncDisposable, IObjectConfiguration
{
    private readonly Dictionary<Type, Entry> entries = new();

    public IObjectProfile? GetProfile(Type target)
        => entries.TryGetValue(target)?.Profile;

    public void Configure(IObjectProfile profile)
    {
        if (entries.TryGetValue(profile.Target, out Entry? entry) && entry.IsSingletonCreationStarted)
        {
            throw new InvalidOperationException();
        }

        entries[profile.Target] = new Entry(this, profile);
    }

    public async ValueTask<object?> TryCreate(Type target, IArgumentGenerator? arguments = null)
    {
        if (TryGetEntry(target, out Entry? entry) && entry.CanBuild)
        {
            return await entry.Build(arguments);
        }

        return null;
    }

    public async ValueTask<object?> TryGet(Type target)
    {
        if (TryGetEntry(target, out Entry? entry) && entry.IsSingleton)
        {
            return await entry.GetSingleton();
        }

        return null;
    }

    public async ValueTask<object?> TryResolve(Type target)
        => await TryGet(target) ?? await TryCreate(target);

    private bool TryGetEntry(Type target, [NotNullWhen(true)] out Entry? entry)
    {
        if (!entries.TryGetValue(target, out entry))
        {
            if (ObjectProfile.Scan(target, out ObjectProfile? profile))
            {
                entry = new Entry(this, profile);
                entries.Add(profile.Target, entry);
            }
            else
            {
                entry = null;
                return false;
            }
        }

        return true;
    }

    private class Entry
    {
        public Entry(IObjectContainer container, IObjectProfile profile)
        {
            this.container = container;
            Profile = profile;

            if (profile.SingletonBuilder != null)
            {
                singleton = new(async () => await profile.SingletonBuilder.Build(container));
            }
        }

        private readonly IObjectContainer container;
        private readonly AsyncLazy<object>? singleton;

        public IObjectProfile Profile { get; }

        public bool IsSingleton => singleton != null;
        public bool IsSingletonCreationStarted => singleton != null && singleton.IsValueCreationStarted;

        public bool CanBuild => Profile.Builder != null;

        public async ValueTask<object> GetSingleton()
            => singleton != null ? await singleton.GetValue() : throw new InvalidOperationException("Not a singleton");

        public async ValueTask<object> Build(IArgumentGenerator? arguments)
            => Profile.Builder != null ? await Profile.Builder.Build(container, arguments) : throw new InvalidOperationException("Cannot be built");
    }
}