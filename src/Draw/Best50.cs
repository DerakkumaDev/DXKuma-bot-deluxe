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
        string bgPath = Path.Combine("Static", "Background", "Best50.png");
        Image image = await Image.LoadAsync(bgPath);
        (int X, int Y) b35Offset = (25, 795);
        (int X, int Y) pos = (0, 0);

        foreach (CommonScore song in b50.Standard!)
        {
            byte[] jacketImgBytes = await Resource.GetJacketAsync(song.Id);
            string pbPath = Path.Combine("Static", "PartBase", $"{song.LevelIndex}.png");
            Image partImg = await Image.LoadAsync(pbPath);
            Image jacketImg = Image.Load(jacketImgBytes);
            jacketImg.Resize(0.56);
            partImg.DrawImage(jacketImg, 36, 41);
            partImg.Resize(340, 110);
            int offsetX = 350 * (pos.X + (pos.Y > 0 ? 0 : 1));
            int offsetY = 120 * pos.Y;
            image.DrawImage(partImg, b35Offset.X + offsetX, b35Offset.Y + offsetY);
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