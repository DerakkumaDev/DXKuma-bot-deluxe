using DXKumaBot.Bot;
using DXKumaBot.Bot.Message;
using System.Text.RegularExpressions;

namespace DXKumaBot.Functions;

public sealed partial class Roll : RegexFunctionBase
{
    protected override async Task Main(object? sender, MessageReceivedEventArgs args)
    {
        MatchCollection matches = MessageRegex().Matches(args.Text);
        List<string> values = [];
        foreach (Match match in matches)
        {
            foreach (Group group in match.Groups)
            {
                if (group.Index is 0)
                {
                    continue;
                }

                foreach (Capture capture in group.Captures)
                {
                    values.Add(capture.Value);
                }
            }
        }

        if (values.Count is 0)
        {
            string filePath = Path.Combine("Static", nameof(Roll), "1.png");
            MediaMessage message = new(MediaType.Photo, filePath);
            await args.Reply(new("没有选项要让迪拉熊怎么选嘛~", message));
        }
        else if (values.Count is 1 || values.All(x => x == values[0]))
        {
            string filePath = Path.Combine("Static", nameof(Roll), "1.png");
            MediaMessage message = new(MediaType.Photo, filePath);
            await args.Reply(new("就一个选项要让迪拉熊怎么选嘛~", message));
        }
        else
        {
            int index = Random.Shared.Next(values.Count);
            string filePath = Path.Combine("Static", nameof(Roll), "0.png");
            MediaMessage message = new(MediaType.Photo, filePath);
            await args.Reply(new($"迪拉熊建议你选择“{values[index]}”呢~", message));
        }
    }

    [GeneratedRegex("(?:.*?是)(.+?)(?:还是(.+?))+",
        RegexOptions.IgnoreCase | RegexOptions.Singleline)]
    protected override partial Regex MessageRegex();
}