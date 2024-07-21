using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace DXKumaBot.Utils;

public static class Draw
{
    public static void DrawImage(this Image image, Image image2, int x, int y)
    {
        image.Mutate(z => z.DrawImage(image2, new Point(x, y), 1));
    }

    public static void Resize(this Image image, double ratio)
    {
        image.Mutate(x => x.Resize(Convert.ToInt32(image.Width * ratio), Convert.ToInt32(image.Height * ratio)));
    }

    public static void Resize(this Image image, int width, int height)
    {
        image.Mutate(x => x.Resize(width, height));
    }
}