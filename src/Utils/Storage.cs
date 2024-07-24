using LiteDB;

namespace DXKumaBot.Utils;

public static class Storage
{
    private static readonly LiteDatabase s_db = new($"{nameof(Storage)}.db");

    public static T? Get<T>(string name, long id)
    {
        ILiteCollection<T> col = s_db.GetCollection<T>(name);
        return col.FindById(id);
    }

    public static bool Set<T>(string name, T value)
    {
        ILiteCollection<T> col = s_db.GetCollection<T>(name);
        return col.Upsert(value);
    }

    public static bool Delete<T>(string name, long id)
    {
        ILiteCollection<T> col = s_db.GetCollection<T>(name);
        return col.Delete(id);
    }
}