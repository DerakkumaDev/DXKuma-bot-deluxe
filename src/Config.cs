using DXKumaBot.Functions.Gallery;
using DXKumaBot.Functions.Interaction;
using DXKumaBot.Utils;

namespace DXKumaBot;

public sealed record Config(CacheConfig Cache, SpecialConfig Special)
{
    public void SetConfig()
    {
        Resource.CacheOutdateDays = Cache.OutdateDays;
        MemberChange.SpecialGroup = Special.GroupUid;
        Pick.SpecialGroup = Special.GroupUid;
        PickNsfw.SpecialGroup = Special.GroupUid;
    }
}

public sealed record CacheConfig(int OutdateDays);

public sealed record SpecialConfig(uint GroupUid, List<uint> NSFWGroups);

public sealed record GalleryConfig(GalleryLimitConfig Limit);

public sealed record GalleryLimitConfig(int Minites, int Times);