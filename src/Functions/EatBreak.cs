using DXKumaBot.Bot.EventArg;
using DXKumaBot.Bot.Message;
using System.Text.RegularExpressions;

namespace DXKumaBot.Functions;

public sealed partial class EatBreak : RegexFunctionBase
{
    private protected override async Task Main(object? sender, MessageReceivedEventArgs args)
    {
        string filePath = Path.Combine("Static", nameof(EatBreak), "0.png");
        MediaMessage message = new(MediaType.Photo, filePath);
        await args.Message.Reply(new("谢谢~", message));
    }

    [GeneratedRegex("(绝赞(给|请)你吃|(给|请)你吃绝赞)", RegexOptions.IgnoreCase | RegexOptions.Singleline)]
    private protected override partial Regex MessageRegex();
}