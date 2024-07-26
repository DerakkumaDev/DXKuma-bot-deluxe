using DXKumaBot.Bot.EventArg;
using DXKumaBot.Utils;
using LiteDB;
using System.Text;
using System.Text.RegularExpressions;

namespace DXKumaBot.Functions.Gallery;

public sealed partial class Rank : RegexFunctionBase
{
    private protected override async Task MainAsync(object? sender, MessageReceivedEventArgs args)
    {
        ILiteCollection<RakingData> col = Storage.GetAll<RakingData>(nameof(Gallery));
        RakingData[] data = [..col.Find(x => x.SourceType == args.Message.SourceType)];
        Array.Sort(data);
        Array.Reverse(data);
        if (data.Length < 1)
        {
            return;
        }

        StringBuilder stringBuilder = new();
        stringBuilder.AppendLine("本周迪拉熊厨力最高的人是……");
        int index = 0;
        foreach (RakingData rakingData in data)
        {
            string userName = await args.Message.Bot.GetUserNameAsync(rakingData.Id, args.Message.ChatId);
            stringBuilder.AppendLine($"{++index}. {userName}：{rakingData.Dates.Count}");
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