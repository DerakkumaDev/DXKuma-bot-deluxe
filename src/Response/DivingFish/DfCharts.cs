using System.Text.Json.Serialization;

namespace DXKumaBot.Response.DivingFish;

public sealed class DfCharts
{
    [JsonPropertyName("dx")] public DfScore[]? Dx { get; set; }

    [JsonPropertyName("sd")] public DfScore[]? Standard { get; set; }
}