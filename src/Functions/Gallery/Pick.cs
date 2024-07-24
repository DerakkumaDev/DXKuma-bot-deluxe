using DXKumaBot.Bot.EventArg;
using DXKumaBot.Bot.Message;
using DXKumaBot.Utils;
using System.Collections.Concurrent;
using System.Text.RegularExpressions;

namespace DXKumaBot.Functions.Gallery;

public sealed partial class Pick : RegexFunctionBase
{
    private readonly ConcurrentDictionary<long, ConcurrentQueue<DateTime>> _groups = new();
    private readonly int _specialGroup = 967611986;

    private protected override async Task MainAsync(object? sender, MessageReceivedEventArgs args)
    {
        if (args.Message.ChatId != _specialGroup)
        {
            if (!_groups.TryGetValue(args.Message.ChatId, out ConcurrentQueue<DateTime>? queue))
            {
                queue = new();
            }

            while (queue.TryPeek(out DateTime dateTime))
            {
                if (args.Message.DateTime - dateTime < TimeSpan.FromMinutes(1))
                {
                    break;
                }

                queue.TryDequeue(out _);
            }

            if (queue.Count > 9)
            {
                _groups[args.Message.ChatId] = queue;
                await args.Message.ReplyAsync(new("迪拉熊提醒你：不要发太多刷屏哦~等下再试吧~"));
                return;
            }

            queue.Enqueue(args.Message.DateTime);
            _groups[args.Message.ChatId] = queue;
        }

        string dirPath = Path.Combine("Static", nameof(Gallery), "SFW");
        string[] files = Directory.GetFiles(dirPath);
        int index = Random.Shared.Next(files.Length);
        MediaMessage message = new(MediaType.Photo, files[index]);
        await args.Message.ReplyAsync(message, true);
        RakingData? data = Storage.Get<RakingData>(nameof(Gallery), args.Message.ChatId);
        if (data is null || !data.Date.IsSameWeek(DateTime.Today))
        {
            data = new()
            {
                Id = args.Message.ChatId,
                Date = args.Message.DateTime,
                Counts = []
            };
        }

        data.Counts.TryGetValue(args.Message.UserId, out int count);
        data.Counts[args.Message.UserId] = count + 1;
        Storage.Set(nameof(Gallery), data);
    }

    [GeneratedRegex("^(随机迪拉熊|dlx)$", RegexOptions.IgnoreCase | RegexOptions.Singleline)]
    private protected override partial Regex MessageRegex();
}