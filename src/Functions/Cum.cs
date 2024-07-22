using DXKumaBot.Bot;
using DXKumaBot.Bot.Message;
using DXKumaBot.Utils;
using System.Text.RegularExpressions;

namespace DXKumaBot.Functions;

public sealed partial class Cum : RegexFunctionBase
{
    protected override async Task Main(object? sender, MessageReceivedEventArgs args)
    {
        int index = Random.Shared.Choose([9, 1]);
        string filePath = Path.Combine("Static", nameof(Cum), $"{index}.png");
        MediaMessage message = new(MediaType.Photo, filePath);
        await args.Reply(message);
    }

    [GeneratedRegex("dlxcum", RegexOptions.IgnoreCase | RegexOptions.Singleline)]
    protected override partial Regex MessageRegex();
}