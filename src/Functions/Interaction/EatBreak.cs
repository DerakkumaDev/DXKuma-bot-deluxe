using DXKumaBot.Bot.EventArg;
using Lagrange.Core.Common.Interface.Api;
using Lagrange.Core.Message;
using System.Text.RegularExpressions;

namespace DXKumaBot.Functions.Interaction;

public sealed partial class EatBreak
{
    public async Task EntryAsync(object? sender, MessageReceivedEventArgs args)
    {
        if (string.IsNullOrEmpty(args.Text.Value))
        {
            return;
        }

        if (!args.MentionedBot)
        {
            return;
        }

        if (!MessageRegex().IsMatch(args.Text.Value))
        {
            return;
        }

        MessageBuilder messageBuilder = MessageBuilder.Group(args.GroupUin);
        messageBuilder.Forward(args.Event.Chain);
        messageBuilder.Text("谢谢~");
        string filePath = Path.Combine("Static", GetType().Name, "0.png");
        messageBuilder.Image(filePath);
        await args.Bot.SendMessage(messageBuilder.Build());
    }

    [GeneratedRegex("(绝赞(给|请)你吃|(给|请)你吃绝赞)", RegexOptions.IgnoreCase | RegexOptions.Singleline)]
    private partial Regex MessageRegex();
}