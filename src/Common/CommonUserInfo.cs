namespace DXKumaBot.Common;

public sealed record CommonUserInfo
{
    public string? Name { get; set; }

    public int CourseRank { get; set; }

    public int ClassRank { get; set; }

    public int IconId { get; set; }

    public int NamePlateId { get; set; }

    public int FrameId { get; set; }
}