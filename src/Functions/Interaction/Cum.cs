using DXKumaBot.Bot.EventArg;
using DXKumaBot.Utils;
using Lagrange.Core.Common.Interface.Api;
using Lagrange.Core.Message;
using System.Text.RegularExpressions;

namespace DXKumaBot.Functions.Interaction;

public sealed partial class Cum : RegexFunctionBase
{
    private readonly WeightsRandom _random = new([9, 1]);

    private protected override async Task MainAsync(object? sender, MessageReceivedEventArgs args)
    {
        MessageBuilder messageBuilder = MessageBuilder.Group(args.GroupUin);
        int index = _random.Next();
        string filePath = Path.Combine("Static", GetType().Name, $"{index}.png");
        messageBuilder.Image(filePath);
        await args.Bot.SendMessage(messageBuilder.Build());
    }

    [GeneratedRegex("^dlxcum$", RegexOptions.IgnoreCase | RegexOptions.Singleline)]
    private protected override partial Regex MessageRegex();
}