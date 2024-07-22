using System.Text.Json.Serialization;

namespace DXKumaBot.Response.Lxns;

public sealed class LxnsCollectionRequired
{
    [JsonPropertyName("difficulties")] public int[]? Difficulties { get; set; }

    [JsonPropertyName("rate")] public LxnsRateType? Rate { get; set; }

    [JsonPropertyName("fc")] public LxnsComboType? Combo { get; set; }

    [JsonPropertyName("fs")] public LxnsSyncType? Sync { get; set; }

    [JsonPropertyName("songs")] public LxnsCollectionRequiredSong[]? Songs { get; set; }

    [JsonPropertyName("completed")] public bool? Completed { get; set; }
}