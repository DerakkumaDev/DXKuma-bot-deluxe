using System.Text.Json.Serialization;

namespace DXKumaBot.Response.Lxns;

public sealed class LxnsCollection
{
    [JsonPropertyName("id")] public int Id { get; set; }

    [JsonPropertyName("name")] public string? Name { get; set; }

    [JsonPropertyName("color")] public string? Color { get; set; }

    [JsonPropertyName("description")] public string? Description { get; set; }

    [JsonPropertyName("genre")] public string? Genre { get; set; }

    [JsonPropertyName("required")] public LxnsCollectionRequired[]? Required { get; set; }
}