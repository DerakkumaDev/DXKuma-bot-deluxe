using DXKumaBot.Bot.EventArg;
using DXKumaBot.Bot.Message;
using Telegram.Bot.Types;

namespace DXKumaBot.Functions;

public sealed class MemberChange
{
    private const int SpecialGroup = 967611986;

    public async Task JoinEntryAsync(object? sender, MembersAddedEventArgs args)
    {
        if (args.SourceType is MessageSource.Qq)
        {
            await args.Reply(new(args.QqMessage!.GroupUin is SpecialGroup
                ? $"恭喜{args.UserName}（{args.UserId}）发现了迪拉熊宝藏地带，发送dlxhelp试一下吧~"
                : $"欢迎{args.UserName}（{args.UserId}）加入本群，发送dlxhelp和迪拉熊一起玩吧~"));
        }

        if (args.SourceType is MessageSource.Telegram)
        {
            foreach (User user in args.TgMessage!.NewChatMembers!)
            {
                await args.Reply(new($"欢迎{user.FirstName}{user.LastName}（{user.Username}）加入本群，发送dlxhelp和迪拉熊一起玩吧~"));
            }
        }
    }

    public async Task LeftEntryAsync(object? sender, MembersLeftEventArgs args)
    {
        await args.Reply(new(args.SourceType is MessageSource.Qq && args.QqMessage!.GroupUin is SpecialGroup
            ? $"很遗憾，{args.UserName}（{args.UserId}）离开了迪拉熊的小窝QAQ"
            : $"{args.UserName}{(string.IsNullOrEmpty(args.UserId) ? $"（{args.UserId}）" : string.Empty)}离开了迪拉熊QAQ"));
    }
}