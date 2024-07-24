using DXKumaBot.Bot.EventArg;
using DXKumaBot.Bot.Message;
using System.Text.RegularExpressions;

namespace DXKumaBot.Functions.Interaction;

public sealed partial class Help : RegexFunctionBase
{
    private protected override async Task MainAsync(object? sender, MessageReceivedEventArgs args)
    {
        string filePath = Path.Combine("Static", nameof(Help), "0.png");
        MediaMessage message = new(MediaType.Photo, filePath);
        if (args.Message.SourceType is MessageSource.Qq)
        {
            await args.Message.ReplyAsync(new("迪拉熊测试群：959231211", message));
            return;
        }

        await args.Message.ReplyAsync(message, true);
    }

    [GeneratedRegex("^((迪拉熊|dlx)(help|指令|帮助)|指令大全)$", RegexOptions.IgnoreCase | RegexOptions.Singleline)]
    private protected override partial Regex MessageRegex();
}