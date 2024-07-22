using DXKumaBot.Bot.EventArg;
using DXKumaBot.Bot.Message;
using DXKumaBot.Utils;
using System.Text.RegularExpressions;

namespace DXKumaBot.Functions;

public sealed partial class Cum : RegexFunctionBase
{
    private protected override async Task Main(object? sender, MessageReceivedEventArgs args)
    {
        int index = Random.Shared.Choose([9, 1]);
        string filePath = Path.Combine("Static", nameof(Cum), $"{index}.png");
        MediaMessage message = new(MediaType.Photo, filePath);
        await args.Message.Reply(message);
    }

    [GeneratedRegex("dlxcum", RegexOptions.IgnoreCase | RegexOptions.Singleline)]
    private protected override partial Regex MessageRegex();
}