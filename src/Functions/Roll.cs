using DXKumaBot.Bot.EventArg;
using DXKumaBot.Bot.Message;
using System.Text.RegularExpressions;

namespace DXKumaBot.Functions;

public sealed partial class Roll
{
    public async Task EntryAsync(object? sender, MessageReceivedEventArgs args)
    {
        if (string.IsNullOrEmpty(args.Message.Text))
        {
            return;
        }

        if (!args.Message.ToBot)
        {
            return;
        }

        MatchCollection matches = MessageRegex().Matches(args.Message.Text);
        List<string> values = [];
        foreach (Match match in matches)
        {
            foreach (Group group in match.Groups)
            {
                values.AddRange(from capture in @group.Captures where @group.Index is not 0 select capture.Value);
            }
        }

        if (values.Count is 0)
        {
            return;
        }

        if (values.Count is 1 || values.All(x => x == values[0]))
        {
            string filePath = Path.Combine("Static", nameof(Roll), "1.png");
            MediaMessage message = new(MediaType.Photo, filePath);
            await args.Message.Reply(new("就一个选项要让迪拉熊怎么选嘛~", message));
        }
        else
        {
            int index = Random.Shared.Next(values.Count);
            string filePath = Path.Combine("Static", nameof(Roll), "0.png");
            MediaMessage message = new(MediaType.Photo, filePath);
            await args.Message.Reply(new($"迪拉熊建议你选择“{values[index]}”呢~", message));
        }
    }

    [GeneratedRegex("(?:.*?是)(.+?)(?:还是(.+?))+",
        RegexOptions.IgnoreCase | RegexOptions.Singleline)]
    private partial Regex MessageRegex();
}