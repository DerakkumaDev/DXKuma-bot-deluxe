using DXKumaBot.Bot.EventArg;
using DXKumaBot.Bot.Message;
using System.Text.RegularExpressions;

namespace DXKumaBot.Functions.Interaction;

public sealed partial class EatBreak
{
    public async Task EntryAsync(object? sender, MessageReceivedEventArgs args)
    {
        if (string.IsNullOrEmpty(args.Message.Text))
        {
            return;
        }

        if (!args.Message.ToBot)
        {
            return;
        }

        if (!MessageRegex().IsMatch(args.Message.Text))
        {
            return;
        }

        string filePath = Path.Combine("Static", nameof(EatBreak), "0.png");
        MediaMessage message = new(MediaType.Photo, filePath);
        await args.Message.ReplyAsync(new("谢谢~", message));
    }

    [GeneratedRegex("(绝赞(给|请)你吃|(给|请)你吃绝赞)", RegexOptions.IgnoreCase | RegexOptions.Singleline)]
    private partial Regex MessageRegex();
}