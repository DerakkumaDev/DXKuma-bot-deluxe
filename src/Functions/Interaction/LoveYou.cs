using DXKumaBot.Bot.EventArg;
using DXKumaBot.Bot.Message;
using System.Text.RegularExpressions;

namespace DXKumaBot.Functions.Interaction;

public sealed partial class LoveYou
{
    public async Task EntryAsync(object? sender, MessageReceivedEventArgs args)
    {
        if (string.IsNullOrEmpty(args.Message.Text))
        {
            return;
        }

        if (args.Message.ToBot && !MessageToBotRegex().IsMatch(args.Message.Text))
        {
            return;
        }

        if (!MessageRegex().IsMatch(args.Message.Text))
        {
            return;
        }

        string filePath = Path.Combine("Static", nameof(LoveYou), "0.png");
        MediaMessage message = new(MediaType.Photo, filePath);
        await args.Message.ReplyAsync(new("迪拉熊也喜欢你❤️", message));
    }

    [GeneratedRegex("^(迪拉熊|dlx)我喜欢你$", RegexOptions.IgnoreCase | RegexOptions.Singleline)]
    private partial Regex MessageRegex();

    [GeneratedRegex("我喜欢你", RegexOptions.IgnoreCase | RegexOptions.Singleline)]
    private partial Regex MessageToBotRegex();
}