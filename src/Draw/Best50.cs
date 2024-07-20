using DXKumaBot.Common;
using DXKumaBot.Response.Lxns;
using DXKumaBot.Utils;
using SixLabors.ImageSharp;

namespace DXKumaBot.Draw;

public static class Best50
{
    public static async Task<MemoryStream> DrawAsync(LxnsB50 b50, LxnsPlayer userInfo, string configPath)
    {
        return await DrawAsync(b50.Convert(), userInfo.Convert(), configPath);
    }

    private static async Task<MemoryStream> DrawAsync(CommonB50 b50, CommonUserInfo userInfo, string configPath)
    {
        Image image = await Image.LoadAsync(Path.Combine("Static", "Background", "Best50.png"));
        (int X, int Y) b35Offset = (25, 795);
        (int X, int Y) pos = (0, 0);

        foreach (CommonScore song in b50.Standard)
        {
            byte[] jacketImgBytes = await Resource.GetJacketAsync(song.Id);
            Image partImg = await Image.LoadAsync(Path.Combine("Static", "PartBase", $"{song.LevelIndex}.png"));
            Image jacketImg = Image.Load(jacketImgBytes);
            jacketImg.Resize(0.56);
            partImg.DrawImage(jacketImg, 36, 41);
            partImg.Resize(340, 110);
            image.DrawImage(partImg, b35Offset.X + 350 * (pos.X + (pos.Y > 0 ? 0 : 1)), b35Offset.Y + 120 * pos.Y);
            ++pos.X;
            if (pos.X < (pos.Y < 1 ? 3 : 4))
            {
                continue;
            }

            pos.X = 0;
            ++pos.Y;
        }
        
        MemoryStream stream = new();
        await image.SaveAsJpegAsync(stream);
        return stream;
    }
}