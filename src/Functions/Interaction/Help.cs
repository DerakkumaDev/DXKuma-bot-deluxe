using DXKumaBot.Bot.EventArg;
using Lagrange.Core.Common.Interface.Api;
using Lagrange.Core.Message;
using System.Text.RegularExpressions;

namespace DXKumaBot.Functions.Interaction;

public sealed partial class Help : RegexFunctionBase
{
    private protected override async Task MainAsync(object? sender, MessageReceivedEventArgs args)
    {
        MessageBuilder messageBuilder = MessageBuilder.Group(args.GroupUin);
        string filePath = Path.Combine("Static", GetType().Name, "0.png");
        messageBuilder.Image(filePath);
        messageBuilder.Text("迪拉熊测试群：959231211");
        await args.Bot.SendMessage(messageBuilder.Build());
    }

    [GeneratedRegex("^((迪拉熊|dlx)(help|指令|帮助)|指令大全)$", RegexOptions.IgnoreCase | RegexOptions.Singleline)]
    private protected override partial Regex MessageRegex();
}