using DXKumaBot.Bot.Message;
using Lagrange.Core.Event.EventArg;

namespace DXKumaBot.Bot.EventArg;

public class MembersAddedEventArgs : EventArgs
{
    private readonly IBot _bot;

    public MembersAddedEventArgs(IBot bot, GroupMemberIncreaseEvent message)
    {
        _bot = bot;
        QqMessage = message;
        SourceType = MessageSource.Qq;
    }

    public MembersAddedEventArgs(IBot bot, TgMessage message)
    {
        _bot = bot;
        TgMessage = message;
        SourceType = MessageSource.Telegram;
    }

    public MessageSource SourceType { get; }
    public GroupMemberIncreaseEvent? QqMessage { get; }
    public TgMessage? TgMessage { get; }

    public long UserId => SourceType switch
    {
        MessageSource.Qq => QqMessage!.MemberUin,
        MessageSource.Telegram => throw new NotSupportedException(),
        _ => throw new ArgumentOutOfRangeException(nameof(SourceType), SourceType, null)
    };

    public string UserName => SourceType switch
    {
        MessageSource.Qq => ((QqBot)_bot).GetUserInfoAsync(QqMessage!.MemberUin).Result!.Nickname,
        MessageSource.Telegram => throw new NotSupportedException(),
        _ => throw new ArgumentOutOfRangeException(nameof(SourceType), SourceType, null)
    };

    public async Task ReplyAsync(MessagePair messages)
    {
        await _bot.SendMessageAsync(messages, SourceType switch
        {
            MessageSource.Qq => QqMessage!.GroupUin,
            MessageSource.Telegram => TgMessage!.Chat.Id,
            _ => throw new ArgumentOutOfRangeException(nameof(SourceType), SourceType, null)
        }, SourceType is MessageSource.Telegram ? TgMessage : default);
    }
}