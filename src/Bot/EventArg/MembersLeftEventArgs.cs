using DXKumaBot.Bot.Message;
using Lagrange.Core.Event.EventArg;

namespace DXKumaBot.Bot.EventArg;

public class MembersLeftEventArgs : EventArgs
{
    private readonly IBot _bot;

    public MembersLeftEventArgs(IBot bot, GroupMemberDecreaseEvent message)
    {
        _bot = bot;
        QqMessage = message;
        SourceType = MessageSource.Qq;
    }

    public MembersLeftEventArgs(IBot bot, TgMessage message)
    {
        _bot = bot;
        TgMessage = message;
        SourceType = MessageSource.Telegram;
    }

    public MessageSource SourceType { get; }
    public GroupMemberDecreaseEvent? QqMessage { get; }
    public TgMessage? TgMessage { get; }

    public string UserId => SourceType switch
    {
        MessageSource.Qq => QqMessage!.MemberUin.ToString(),
        MessageSource.Telegram => TgMessage!.LeftChatMember!.Username,
        _ => throw new ArgumentOutOfRangeException(nameof(SourceType), SourceType, null)
    } ?? throw new NullReferenceException();

    public string UserName => SourceType switch
    {
        MessageSource.Qq => ((QqBot)_bot).GetUserInfo(QqMessage!.MemberUin).Result!.Nickname,
        MessageSource.Telegram => $"{TgMessage!.LeftChatMember!.FirstName}{TgMessage!.LeftChatMember!.LastName}",
        _ => throw new ArgumentOutOfRangeException(nameof(SourceType), SourceType, null)
    };

    public async Task Reply(MessagePair messages)
    {
        await _bot.SendMessageAsync(messages, SourceType switch
        {
            MessageSource.Qq => QqMessage!.GroupUin,
            MessageSource.Telegram => TgMessage!.Chat.Id,
            _ => throw new ArgumentOutOfRangeException(nameof(SourceType), SourceType, null)
        }, SourceType is MessageSource.Telegram ? TgMessage : default);
    }
}