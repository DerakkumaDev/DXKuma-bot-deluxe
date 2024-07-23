using DXKumaBot.Bot.EventArg;
using DXKumaBot.Bot.Message;
using Telegram.Bot.Types;

namespace DXKumaBot.Functions;

public sealed class MemberChange
{
    private readonly int _specialGroup = 967611986;

    public async Task JoinEntryAsync(object? sender, MembersAddedEventArgs args)
    {
        string filePath = Path.Combine("Static", nameof(MemberChange), "0.png");
        MediaMessage message = new(MediaType.Photo, filePath);
        switch (args.SourceType)
        {
            case MessageSource.Qq:
                await args.Reply(new(args.QqMessage!.GroupUin == _specialGroup
                    ? $"恭喜{args.UserName}（{args.UserId}）发现了迪拉熊宝藏地带，发送dlxhelp试一下吧~"
                    : $"欢迎{args.UserName}（{args.UserId}）加入本群，发送dlxhelp和迪拉熊一起玩吧~", message));
                break;
            case MessageSource.Telegram:
                foreach (User user in args.TgMessage!.NewChatMembers!)
                {
                    await args.Reply(new($"欢迎{user.FirstName}{user.LastName}（{user.Username}）加入本群，发送dlxhelp和迪拉熊一起玩吧~"));
                }

                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(args));
        }
    }

    public async Task LeftEntryAsync(object? sender, MembersLeftEventArgs args)
    {
        string filePath = Path.Combine("Static", nameof(MemberChange), "1.png");
        MediaMessage message = new(MediaType.Photo, filePath);
        await args.Reply(new(args.SourceType is MessageSource.Qq && args.QqMessage!.GroupUin == _specialGroup
                ? $"很遗憾，{args.UserName}（{args.UserId}）离开了迪拉熊的小窝QAQ"
                : $"{args.UserName}{(string.IsNullOrEmpty(args.UserId) ? $"（{args.UserId}）" : string.Empty)}离开了迪拉熊QAQ",
            message));
    }
}