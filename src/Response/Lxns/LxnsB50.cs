using DXKumaBot.Common;
using System.Text.Json.Serialization;

namespace DXKumaBot.Response.Lxns;

public sealed class LxnsB50
{
    [JsonPropertyName("standard_total")] public int StandardTotal { get; set; }

    [JsonPropertyName("dx_total")] public int DxTotal { get; set; }

    [JsonPropertyName("standard")] public LxnsScore[]? Standard { get; set; }

    [JsonPropertyName("dx")] public LxnsScore[]? Dx { get; set; }

    public static implicit operator CommonB50(LxnsB50 lxB50)
    {
        CommonB50 b50 = new()
        {
            Standard = [],
            Dx = []
        };
        foreach (LxnsScore score in lxB50.Standard!)
        {
            CommonScore commonScore = new()
            {
                Id = score.Id,
                Achievements = score.Achievements,
                Combo = score.Combo,
                DxScore = score.DxScore,
                LevelIndex = score.LevelIndex,
                Sync = score.Sync,
                Type = score.Type
            };
            b50.Standard.Add(commonScore);
        }

        foreach (LxnsScore score in lxB50.Dx!)
        {
            CommonScore commonScore = new()
            {
                Id = score.Id,
                Achievements = score.Achievements,
                Combo = score.Combo,
                DxScore = score.DxScore,
                LevelIndex = score.LevelIndex,
                Sync = score.Sync,
                Type = score.Type
            };
            b50.Dx.Add(commonScore);
        }

        return b50;
    }
}