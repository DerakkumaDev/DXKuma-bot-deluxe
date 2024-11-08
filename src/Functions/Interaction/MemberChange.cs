using DXKumaBot.Bot.EventArg;
using Lagrange.Core.Common.Interface.Api;
using Lagrange.Core.Message;

namespace DXKumaBot.Functions.Interaction;

public sealed class MemberChange
{
    static MemberChange()
    {
        SpecialGroup = 0;
    }

    public static uint SpecialGroup { private get; set; }

    public async Task JoinEntryAsync(object? sender, MembersAddedEventArgs args)
    {
        MessageBuilder messageBuilder = MessageBuilder.Group(args.Event.GroupUin);
        messageBuilder.Text(args.Event.GroupUin == SpecialGroup
            ? $"恭喜{args.MemberName}（{args.MemberQid ?? args.Event.MemberUin.ToString()}）发现了迪拉熊宝藏地带，发送dlxhelp试一下吧~"
            : $"欢迎{args.MemberName}（{args.MemberQid ?? args.Event.MemberUin.ToString()}）加入本群，发送dlxhelp和迪拉熊一起玩吧~");
        string filePath = Path.Combine("Static", GetType().Name, "0.png");
        messageBuilder.Image(filePath);
        await args.Bot.SendMessage(messageBuilder.Build());
    }

    public async Task LeftEntryAsync(object? sender, MembersLeftEventArgs args)
    {
        MessageBuilder messageBuilder = MessageBuilder.Group(args.Event.GroupUin);
        messageBuilder.Text(args.Event.GroupUin == SpecialGroup
            ? $"很遗憾，{args.MemberName}（{args.MemberQid ?? args.Event.MemberUin.ToString()}）离开了迪拉熊的小窝QAQ"
            : $"{args.MemberName}（{args.MemberQid ?? args.Event.MemberUin.ToString()}）离开了迪拉熊QAQ");
        string filePath = Path.Combine("Static", GetType().Name, "1.png");
        messageBuilder.Image(filePath);
        await args.Bot.SendMessage(messageBuilder.Build());
    }
}