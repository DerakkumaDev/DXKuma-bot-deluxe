using DXKumaBot.Bot.EventArg;
using DXKumaBot.Bot.Message;
using DXKumaBot.Utils;
using System.Text.RegularExpressions;

namespace DXKumaBot.Functions.Interaction;

public sealed partial class Cum : RegexFunctionBase
{
    private protected override async Task MainAsync(object? sender, MessageReceivedEventArgs args)
    {
        int index = Random.Shared.Choose([9, 1]);
        string filePath = Path.Combine("Static", nameof(Cum), $"{index}.png");
        MediaMessage message = new(MediaType.Photo, filePath);
        await args.Message.ReplyAsync(message);
    }

    [GeneratedRegex("dlxcum", RegexOptions.IgnoreCase | RegexOptions.Singleline)]
    private protected override partial Regex MessageRegex();
}