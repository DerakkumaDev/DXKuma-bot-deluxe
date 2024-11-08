using DXKumaBot.Bot.EventArg;
using Lagrange.Core.Common.Interface.Api;
using Lagrange.Core.Message;
using System.Text.RegularExpressions;

namespace DXKumaBot.Functions.Gallery;

public sealed partial class Pick : RegexFunctionBase
{
    private readonly Dictionary<long, Queue<DateTime>> _groups = new();

    static Pick()
    {
        SpecialGroup = 0;
        LimitMinutes = 1;
        LimitTimes = 10;
    }

    public static uint SpecialGroup { private get; set; }
    public static uint LimitMinutes { private get; set; }
    public static uint LimitTimes { private get; set; }

    private protected override async Task MainAsync(object? sender, MessageReceivedEventArgs args)
    {
        MessageBuilder messageBuilder = MessageBuilder.Group(args.GroupUin);
        if (args.GroupUin != SpecialGroup && !CheckInterval(_groups, args.GroupUin, args.Event.Chain.Time))
        {
            messageBuilder.Text("迪拉熊提醒你：不要发太多刷屏哦~等下再试吧~");
        }
        else
        {
            string dirPath = Path.Combine("Static", nameof(Gallery), "SFW");
            string[] files = Directory.GetFiles(dirPath);
            if (files.Length <= 0)
            {
                messageBuilder.Text("迪拉熊不准你看");
                string picPath = Path.Combine("Static", nameof(Gallery), "0.png");
                messageBuilder.Image(picPath);
                await args.Bot.SendMessage(messageBuilder.Build());
                return;
            }

            int index = Random.Shared.Next(files.Length);
            messageBuilder.Image(files[index]);
        }

        await args.Bot.SendMessage(messageBuilder.Build());
        RakingData.Update(args.Event.Chain);
    }

    public static bool CheckInterval(IDictionary<long, Queue<DateTime>> group, long id, DateTime now)
    {
        if (!group.TryGetValue(id, out Queue<DateTime>? queue))
        {
            queue = new();
        }

        while (queue.TryPeek(out DateTime dateTime))
        {
            if (now - dateTime < TimeSpan.FromMinutes(LimitMinutes))
            {
                break;
            }

            queue.TryDequeue(out _);
        }

        if (queue.Count > LimitTimes - 1)
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