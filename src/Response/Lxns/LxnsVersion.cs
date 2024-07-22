using System.Text.Json.Serialization;

namespace DXKumaBot.Response.Lxns;

public sealed class LxnsVersion
{
    [JsonPropertyName("id")] public int Id { get; set; }

    [JsonPropertyName("title")] public string? Title { get; set; }

    [JsonPropertyName("version")] public int Version { get; set; }
}