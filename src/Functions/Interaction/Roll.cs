using DXKumaBot.Bot.EventArg;
using Lagrange.Core.Common.Interface.Api;
using Lagrange.Core.Message;
using System.Text.RegularExpressions;

namespace DXKumaBot.Functions.Interaction;

public sealed partial class Roll
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

        MatchCollection matches = MessageRegex().Matches(args.Text.Value);
        List<string> values = [];
        foreach (Match match in matches)
        {
            foreach (Group group in match.Groups.Cast<Group>())
            {
                values.AddRange(from capture in @group.Captures where @group.Index is not 0 select capture.Value);
            }
        }

        if (values.Count is 0)
        {
            return;
        }

        MessageBuilder messageBuilder = MessageBuilder.Group(args.GroupUin);
        if (values.Count is 1 || values.All(x => x == values[0]))
        {
            messageBuilder.Text("就一个选项要让迪拉熊怎么选嘛~");
            string filePath = Path.Combine("Static", GetType().Name, "1.png");
            messageBuilder.Image(filePath);
        }
        else
        {
            int index = Random.Shared.Next(values.Count);
            messageBuilder.Text($"迪拉熊建议你选择“{values[index]}”呢~");
            string filePath = Path.Combine("Static", GetType().Name, "0.png");
            messageBuilder.Image(filePath);
        }

        await args.Bot.SendMessage(messageBuilder.Build());
    }

    [GeneratedRegex("(?:.*?是)(.+?)(?:还是(.+?))+", RegexOptions.IgnoreCase | RegexOptions.Singleline)]
    private partial Regex MessageRegex();
}