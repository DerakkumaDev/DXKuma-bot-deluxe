using System.Text.Json.Serialization;

namespace DXKumaBot.Response.Lxns;

public sealed record LxnsResponse<T>
{
    [JsonPropertyName("success")] public bool Success { get; set; }

    [JsonPropertyName("code")] public int Code { get; set; }

    [JsonPropertyName("message")] public string? Message { get; set; }

    [JsonPropertyName("data")] public T? Data { get; set; }
}