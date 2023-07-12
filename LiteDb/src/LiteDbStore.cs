namespace Markwardt;

public class LiteDbStore : IEntityStore
{
    public LiteDbStore(Path path)
    {
        BsonMapper mapper = new();
        mapper.Entity<IIdentifiable>().Id(x => x.Id);
        entries = new LiteDatabase(path.ToString("/"), mapper).GetCollection<IIdentifiable>("Entities");
    }

    private readonly ILiteCollection<IIdentifiable> entries;
    private readonly OperationQueue operations = new();

    public async ValueTask Save(IEnumerable<IIdentifiable> targets)
    {
        List<IIdentifiable> toSave = targets.ToList();
        await operations.EnqueueThread(() =>
        {
            foreach (IIdentifiable obj in toSave)
            {
                entries.Upsert(obj);
            }
        });
    }

    public async ValueTask<IEnumerable<IIdentifiable>> Load(IEnumerable<string> ids)
    {
        List<string> toLoad = ids.ToList();
        List<IIdentifiable> entities = new();
        await operations.EnqueueThread(() =>
        {
            foreach (string id in toLoad)
            {
                IIdentifiable? entity = entries.FindById(id);
                if (entity != null)
                {
                    entities.Add(entity);
                }
            }
        });

        return entities;
    }

    public async ValueTask Delete(IEnumerable<string> ids)
    {
        List<string> toDelete = ids.ToList();
        await operations.EnqueueThread(() =>
        {
            foreach (string id in toDelete)
            {
                entries.Delete(id);
            }
        });
    }

    private async Task Execute(Action action)
        => await operations.EnqueueThread(action);

    private async Task<T> Execute<T>(Func<T> action)
        => await operations.EnqueueThread(action);
}

/*public class LiteDbStore : IEntityStore
{
    protected static ObjectId GetObjectId(string id)
        => new ObjectId(id.ToLower());

    protected static string GetId(ObjectId id)
        => id.ToString().ToUpper();

    public LiteDbStore(IJsonSerializer serializer, string path)
    {
        Database = new(path);
        this.serializer = serializer;
        entries = Database.GetCollection<BsonDocument>("Objects");
    }

    protected LiteDatabase Database { get; }

    private readonly IJsonSerializer serializer;
    private readonly ILiteCollection<BsonDocument> entries;
    private readonly OperationQueue operations = new();

    public string GenerateId()
        => GetId(ObjectId.NewObjectId());

    public async Task<bool> Contains(IEnumerable<string> ids)
    {
        List<string> toContains = ids.ToList();
        return await Execute(() => toContains.Any(i => entries.FindById(GetObjectId(i)) == null));
    }

    public async Task Save(IEnumerable<KeyValuePair<string, object>> objects)
    {
        List<KeyValuePair<string, object>> toSave = objects.ToList();
        await Execute(() =>
        {
            foreach (KeyValuePair<string, object> obj in toSave)
            {
                entries.Upsert(Serialize(obj));
            }
        });
    }

    public async Task<IDictionary<string, object>> TryLoad(IEnumerable<string> ids)
    {
        List<string> toLoad = ids.ToList();
        Dictionary<string, object> entities = new();
        await Execute(() =>
        {
            foreach (string id in toLoad)
            {
                BsonDocument? document = entries.FindById(GetObjectId(id));
                if (document != null)
                {
                    KeyValuePair<string, object> obj = Deserialize(document);
                    entities.Add(obj.Key, obj.Value);
                }
            }
        });

        return entities;
    }

    public async Task Delete(IEnumerable<string> ids)
    {
        List<string> toDelete = ids.ToList();
        await Execute(() =>
        {
            foreach (string id in toDelete)
            {
                entries.Delete(GetObjectId(id));
            }
        });
    }

    public void Dispose()
        => Database.Dispose();

    public async ValueTask DisposeAsync()
    {
        await operations.Completion;
        Database.Dispose();
    }

    protected async Task Execute(Action action)
        => await operations.EnqueueThread(action);

    protected async Task<T> Execute<T>(Func<T> action)
        => await operations.EnqueueThread(action);

    protected BsonDocument Serialize(KeyValuePair<string, object> obj)
    {
        string json = serializer.Serialize(obj.Value);
        BsonDocument document = LiteDB.JsonSerializer.Deserialize(json).AsDocument;
        document["_id"] = GetObjectId(obj.Key);
        return document;
    }

    protected KeyValuePair<string, object> Deserialize(BsonDocument document)
    {
        string id = GetId(document["_id"]);
        document.Remove("_id");
        string json = LiteDB.JsonSerializer.Serialize(document);
        object value = serializer.Deserialize(json);
        return new KeyValuePair<string, object>(id, value);
    }
}

public class LiteDbStore<TSingleton> : LiteDbStore, IObjectStore<TSingleton>
    where TSingleton : notnull
{
    public static async Task<LiteDbStore<TSingleton>> Create(IJsonSerializer serializer, string path, TSingleton singleton)
    {
        LiteDbStore<TSingleton> store = new(serializer, path);
        store.singleton = new KeyValuePair<string, object>("000000000000000000000000", singleton);
        await store.SaveSingleton();
        return store;
    }

    public static async Task<LiteDbStore<TSingleton>> Load(IJsonSerializer serializer, string path)
    {
        LiteDbStore<TSingleton> store = new(serializer, path);
        await store.LoadSingleton();
        return store;
    }

    private LiteDbStore(IJsonSerializer serializer, string path)
        : base(serializer, path)
    {
        singletonCollection = Database.GetCollection<BsonDocument>("Singleton");
    }

    private readonly ILiteCollection<BsonDocument> singletonCollection;

    private KeyValuePair<string, object> singleton;

    public string SingletonId => singleton.Key;
    public TSingleton Singleton => (TSingleton)singleton.Value;

    public async Task SaveSingleton()
        => await Execute(() => singletonCollection.Upsert(Serialize(singleton)));

    private async Task LoadSingleton()
        => singleton = await Execute(() => Deserialize(singletonCollection.FindAll().First()));
}*/