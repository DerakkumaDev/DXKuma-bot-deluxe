using DXKumaBot.Bot.EventArg;
using DXKumaBot.Bot.Message;
using System.Collections.Concurrent;
using System.Text.RegularExpressions;

namespace DXKumaBot.Functions.Gallery;

public sealed partial class Pick : RegexFunctionBase
{
    private readonly ConcurrentDictionary<long, ConcurrentQueue<DateTime>> _groups = new();
    private readonly int _specialGroup = 967611986;

    private protected override async Task MainAsync(object? sender, MessageReceivedEventArgs args)
    {
        if (args.Message.ChatId != _specialGroup && !CheckInterval(_groups, args.Message.ChatId, args.Message.DateTime))
        {
            await args.Message.ReplyAsync(new("迪拉熊提醒你：不要发太多刷屏哦~等下再试吧~"));
            return;
        }

        string dirPath = Path.Combine("Static", nameof(Gallery), "SFW");
        string[] files = Directory.GetFiles(dirPath);
        int index = Random.Shared.Next(files.Length);
        MediaMessage message = new(MediaType.Photo, files[index]);
        await args.Message.ReplyAsync(message, true);
        RakingData.Update(args.Message);
    }

    public static bool CheckInterval(IDictionary<long, ConcurrentQueue<DateTime>> group, long id, DateTime now)
    {
        const int minutes = 1;
        const int count = 10;
        if (!group.TryGetValue(id, out ConcurrentQueue<DateTime>? queue))
        {
            queue = new();
        }

        while (queue.TryPeek(out DateTime dateTime))
        {
            if (now - dateTime < TimeSpan.FromMinutes(minutes))
            {
                break;
            }

            queue.TryDequeue(out _);
        }

        if (queue.Count > count - 1)
        {
            group[id] = queue;
            return false;
        }

        queue.Enqueue(now);
        group[id] = queue;
        return true;
    }

    [GeneratedRegex("^(随机迪拉熊|dlx)$", RegexOptions.IgnoreCase | RegexOptions.Singleline)]
    private protected override partial Regex MessageRegex();
}