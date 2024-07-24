using DXKumaBot.Common;
using System.Text.Json.Serialization;

namespace DXKumaBot.Response.Lxns;

public sealed record LxnsPlayer
{
    [JsonPropertyName("name")] public string? Name { get; set; }

    [JsonPropertyName("rating")] public int Rating { get; set; }

    [JsonPropertyName("friend_code")] public long FriendCode { get; set; }

    [JsonPropertyName("trophy")] public LxnsCollection? Trophy { get; set; }

    [JsonPropertyName("trophy_name")] public string? TrophyName { get; set; }

    [JsonPropertyName("course_rank")] public int CourseRank { get; set; }

    [JsonPropertyName("class_rank")] public int ClassRank { get; set; }

    [JsonPropertyName("star")] public int Star { get; set; }

    [JsonPropertyName("icon")] public LxnsCollection? Icon { get; set; }

    [JsonPropertyName("name_plate")] public LxnsCollection? NamePlate { get; set; }

    [JsonPropertyName("frame")] public LxnsCollection? Frame { get; set; }

    [JsonPropertyName("upload_time")] public string? UploadTime { get; set; }

    public static implicit operator CommonUserInfo(LxnsPlayer lxUser)
    {
        return new()
        {
            ClassRank = lxUser.ClassRank,
            CourseRank = lxUser.CourseRank,
            FrameId = lxUser.Frame?.Id ?? 0,
            IconId = lxUser.Icon?.Id ?? 0,
            Name = lxUser.Name,
            NamePlateId = lxUser.NamePlate?.Id ?? 0
        };
    }
}