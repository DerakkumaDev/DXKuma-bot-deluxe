namespace DXKumaBot.Utils;

public static class Resource
{
    public enum DataSource
    {
        Default,
        Lxns,
        DivingFish,
        Local = -1
    }

    public enum ResourceType
    {
        Icon,
        Plate,
        Jacket
    }

    private const string BaseUrl = "https://assets2.lxns.net/maimai";
    private const string BaseUrl2 = "https://www.diving-fish.com";
    private const string CachePathName = "Cache";

    static Resource()
    {
        if (!Directory.Exists(CachePathName))
        {
            Directory.CreateDirectory(CachePathName);
        }

        CacheOutdateDays = -1;
    }

    public static int CacheOutdateDays { private get; set; }

    private static bool CheckAvailableCache(string type, int id)
    {
        string path = Path.Combine(type, $"{id}.png");
        string fullPath = Path.Combine(CachePathName, path);
        if (!File.Exists(fullPath))
        {
            return false;
        }

        if (CacheOutdateDays < 0)
        {
            return true;
        }

        Storage storage = Storage.GetFromName(CachePathName);
        Cache? cache = storage.Get<Cache>(type, id);
        if (cache is not null && DateTime.UtcNow - cache.CacheTime < TimeSpan.FromDays(CacheOutdateDays))
        {
            return true;
        }

        File.Delete(fullPath);
        return false;
    }

    public static async Task<byte[]> GetAsync(ResourceType type, int id)
    {
        return await GetAsync(type, id, DataSource.Default);
    }

    public static async Task<byte[]> GetAsync(ResourceType type, int id, DataSource source)
    {
        string baseUrl = BaseUrl;
        string typeName = Enum.GetName(type)!.ToLower();
        string idStr = id.ToString();

        if (source is DataSource.DivingFish && type is not ResourceType.Jacket)
        {
            throw new NotSupportedException();
        }

        if (source is DataSource.Default or DataSource.DivingFish && type is ResourceType.Jacket)
        {
            baseUrl = BaseUrl2;
            typeName = "covers";
            idStr = id.ToString("D5");
        }

        string dirPath = Path.Combine(CachePathName, typeName);
        string path = Path.Combine(dirPath, $"{id}.png");
        if (CheckAvailableCache(typeName, id))
        {
            return await File.ReadAllBytesAsync(path);
        }

        using HttpClient httpClient = new();
        byte[] img = await httpClient.GetByteArrayAsync($"{baseUrl}/{typeName}/{idStr}.png");
        if (!Directory.Exists(dirPath))
        {
            Directory.CreateDirectory(dirPath);
        }

        await File.WriteAllBytesAsync(path, img);
        Storage storage = Storage.GetFromName(CachePathName);
        storage.Set(Enum.GetName(type)!, new Cache
        {
            Id = id,
            CacheTime = DateTime.UtcNow
        });
        return img;
    }

    private class Cache
    {
        public int Id { get; set; }
        public DateTime CacheTime { get; set; }
    }
}