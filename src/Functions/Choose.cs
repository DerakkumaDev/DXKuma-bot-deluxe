using DXKumaBot.Bot;
using DXKumaBot.Bot.Message;
using System.Text.RegularExpressions;

namespace DXKumaBot.Functions;

public sealed partial class Choose : RegexFunctionBase
{
    protected override async Task Main(object? sender, MessageReceivedEventArgs args)
    {
        Match match = MessageRegex().Match(args.Text);
        HashSet<string> values = [];
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

        switch (values.Count)
        {
            case 0:
            {
                string filePath = Path.Combine("Static", nameof(Choose), "1.png");
                MediaMessage message = new(MediaType.Photo, filePath);
                await args.Reply(new("没有选项要让迪拉熊怎么选嘛~", message));
                break;
            }
            case 1:
            {
                string filePath = Path.Combine("Static", nameof(Choose), "1.png");
                MediaMessage message = new(MediaType.Photo, filePath);
                await args.Reply(new("就一个选项要让迪拉熊怎么选嘛~", message));
                break;
            }
            default:
            {
                int index = Random.Shared.Next(values.Count);
                string filePath = Path.Combine("Static", nameof(Choose), "0.png");
                MediaMessage message = new(MediaType.Photo, filePath);
                await args.Reply(new($"迪拉熊建议你选择“{values.ElementAt(index)}”呢~", message));
                break;
            }
        }
    }

    [GeneratedRegex("^(?:.*?是)(.+?)(?:还是(.+?))+$",
        RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Singleline)]
    protected override partial Regex MessageRegex();
}