using DXKumaBot.Bot.EventArg;
using DXKumaBot.Utils;
using System.Text;
using System.Text.RegularExpressions;

namespace DXKumaBot.Functions.Gallery;

public sealed partial class Rank : RegexFunctionBase
{
    private protected override async Task MainAsync(object? sender, MessageReceivedEventArgs args)
    {
        RakingData? data = Storage.Get<RakingData>(nameof(Gallery), args.Message.ChatId);
        if (data is null)
        {
            await args.Message.ReplyAsync(new("空的打哟~"));
            return;
        }

        if (!data.Date.IsSameWeek(DateTime.Today))
        {
            Storage.Delete<RakingData>(nameof(Gallery), args.Message.ChatId);
            await args.Message.ReplyAsync(new("空的打哟~"));
            return;
        }

        StringBuilder stringBuilder = new();
        stringBuilder.AppendLine("本周迪拉熊厨力最高的人是……");
        int index = 0;
        foreach ((long userId, int count) in data.Counts)
        {
            string userName = await args.Message.Bot.GetUserName(userId, args.Message.ChatId);
            stringBuilder.AppendLine($"{++index}. {userName}：{count}");
            if (index > 4)
            {
                break;
            }
        }

        stringBuilder.AppendLine($"迪拉熊给上面{index}个宝宝一个大大的拥抱~");
        stringBuilder.AppendLine("（积分每周一重算）");
        string message = stringBuilder.ToString();
        await args.Message.ReplyAsync(new(message), true);
    }

    [GeneratedRegex("^(迪拉熊|dlx)(排行榜|list)$", RegexOptions.IgnoreCase | RegexOptions.Singleline)]
    private protected override partial Regex MessageRegex();
}