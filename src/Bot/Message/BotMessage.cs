using Lagrange.Core.Message;
using Lagrange.Core.Message.Entity;
using Telegram.Bot.Types.Enums;

namespace DXKumaBot.Bot.Message;

public sealed class BotMessage
{
    private readonly IBot _bot;
    private readonly uint? _groupId;
    private readonly uint? _targetId;

    public BotMessage(IBot bot, MessageChain message)
    {
        _bot = bot;
        QqMessage = message;
        SourceType = MessageSource.Qq;
    }

    public BotMessage(IBot bot, uint groupId, uint targetId)
    {
        _bot = bot;
        _groupId = groupId;
        _targetId = targetId;
        SourceType = MessageSource.Qq;
    }

    public BotMessage(IBot bot, TgMessage message)
    {
        _bot = bot;
        TgMessage = message;
        SourceType = MessageSource.Telegram;
    }

    public MessageSource SourceType { get; }
    public MessageChain? QqMessage { get; }
    public TgMessage? TgMessage { get; }

    public long ChatId => SourceType switch
    {
        MessageSource.Qq => _groupId ?? QqMessage!.GroupUin!.Value,
        MessageSource.Telegram => TgMessage!.Chat.Id,
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
        MessageSource.Qq => (_targetId ?? QqMessage!.TargetUin) == ((QqBot)_bot).Id,
        MessageSource.Telegram => (from item in TgMessage!.Entities!
            where item.Type is MessageEntityType.Mention
            select item).Any(x => TgMessage.Text?[x.Offset..x.Length] == $"@{((TgBot)_bot).UserName}"),
        _ => throw new ArgumentOutOfRangeException(nameof(SourceType), SourceType, null)
    };

    public async Task Reply(MessagePair messages, bool noReply = false)
    {
        await _bot.SendMessageAsync(messages, this, noReply);
    }
}