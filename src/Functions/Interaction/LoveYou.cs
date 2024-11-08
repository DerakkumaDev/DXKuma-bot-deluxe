using DXKumaBot.Bot.EventArg;
using Lagrange.Core.Common.Interface.Api;
using Lagrange.Core.Message;
using System.Text.RegularExpressions;

namespace DXKumaBot.Functions.Interaction;

public sealed partial class LoveYou
{
    public async Task EntryAsync(object? sender, MessageReceivedEventArgs args)
    {
        if (string.IsNullOrEmpty(args.Text.Value))
        {
            return;
        }

        if (args.MentionedBot && !MessageToBotRegex().IsMatch(args.Text.Value))
        {
            return;
        }

        if (!MessageRegex().IsMatch(args.Text.Value))
        {
            return;
        }

        MessageBuilder messageBuilder = MessageBuilder.Group(args.GroupUin);
        messageBuilder.Forward(args.Event.Chain);
        messageBuilder.Text("迪拉熊也喜欢你❤️");
        string filePath = Path.Combine("Static", GetType().Name, "0.png");
        messageBuilder.Image(filePath);
        await args.Bot.SendMessage(messageBuilder.Build());
    }

    [GeneratedRegex("^(迪拉熊|dlx)我喜欢你$", RegexOptions.IgnoreCase | RegexOptions.Singleline)]
    private partial Regex MessageRegex();

    [GeneratedRegex("我喜欢你", RegexOptions.IgnoreCase | RegexOptions.Singleline)]
    private partial Regex MessageToBotRegex();
}