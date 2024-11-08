using LiteDB;

namespace DXKumaBot.Utils;

public sealed class Storage
{
    private static readonly Dictionary<string, Storage> _storages;
    private readonly LiteDatabase _db;

    static Storage()
    {
        _storages = [];
    }

    private Storage(string name)
    {
        _db = new($"{name}.db");
    }

    public static Storage GetFromName(string name)
    {
        if (!_storages.TryGetValue(name, out Storage? storage))
        {
            storage = new(name);
            _storages.Add(name, storage);
        }

        return storage;
    }

    public ILiteCollection<T> GetAll<T>(string key)
    {
        ILiteCollection<T> col = _db.GetCollection<T>(key);
        return col;
    }

    public T? Get<T>(string key, long id)
    {
        ILiteCollection<T> col = _db.GetCollection<T>(key);
        return col.FindById(id);
    }

    public bool Set<T>(string key, T value)
    {
        ILiteCollection<T> col = _db.GetCollection<T>(key);
        return col.Upsert(value);
    }

    public bool Delete<T>(string key, long id)
    {
        ILiteCollection<T> col = _db.GetCollection<T>(key);
        return col.Delete(id);
    }
}