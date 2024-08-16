using Lagrange.Core.Message;
using Lagrange.Core.Message.Entity;
using Telegram.Bot.Types.Enums;

namespace DXKumaBot.Bot.Message;

public sealed record BotMessage
{
    private readonly uint? _groupId;
    private readonly uint? _targetId;

    public BotMessage(IBot bot, MessageChain message)
    {
        Bot = bot;
        QqMessage = message;
        SourceType = MessageSource.Qq;
    }

    public BotMessage(IBot bot, uint groupId, uint targetId)
    {
        Bot = bot;
        _groupId = groupId;
        _targetId = targetId;
        SourceType = MessageSource.Qq;
    }

    public BotMessage(IBot bot, TgMessage message)
    {
        Bot = bot;
        TgMessage = message;
        SourceType = MessageSource.Telegram;
    }

    public IBot Bot { get; }
    public MessageSource SourceType { get; }
    public MessageChain? QqMessage { get; }
    public TgMessage? TgMessage { get; }

    public long ChatId => SourceType switch
    {
        MessageSource.Qq => _groupId ?? QqMessage!.GroupUin!.Value,
        MessageSource.Telegram => TgMessage!.Chat.Id,
        _ => throw new ArgumentOutOfRangeException(nameof(SourceType), SourceType, null)
    };

    public long UserId => SourceType switch
    {
        MessageSource.Qq => _groupId ?? QqMessage!.FriendUin,
        MessageSource.Telegram => TgMessage!.From!.Id,
        _ => throw new ArgumentOutOfRangeException(nameof(SourceType), SourceType, null)
    };

    public string? Text => SourceType switch
    {
        MessageSource.Qq => QqMessage!.GetEntity<TextEntity>()?.Text,
        MessageSource.Telegram => TgMessage!.Text,
        _ => throw new ArgumentOutOfRangeException(nameof(SourceType), SourceType, null)
    };

    public bool ToBot => SourceType switch
    {
        MessageSource.Qq => (_targetId ?? QqMessage!.TargetUin) == ((QqBot)Bot).Id,
        MessageSource.Telegram => (from item in TgMessage!.Entities!
            where item.Type is MessageEntityType.Mention
            select item).Any(x => TgMessage.Text?[x.Offset..x.Length] == $"@{((TgBot)Bot).UserName}"),
        _ => throw new ArgumentOutOfRangeException(nameof(SourceType), SourceType, null)
    };

    public DateTime DateTime => SourceType switch
    {
        MessageSource.Qq => QqMessage!.Time,
        MessageSource.Telegram => TgMessage!.Date,
        _ => throw new ArgumentOutOfRangeException(nameof(SourceType), SourceType, null)
    };

    public async Task<BotMessage> ReplyAsync(MessagePair messages)
    {
        return await Bot.SendMessageAsync(messages, this, false);
    }

    public async Task<BotMessage> ReplyAsync(MessagePair messages, bool noReply)
    {
        return await Bot.SendMessageAsync(messages, this, noReply);
    }
}