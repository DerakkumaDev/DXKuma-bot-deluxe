using Lagrange.Core.Message;
using Lagrange.Core.Message.Entity;

namespace DXKumaBot.Bot.Message;

public sealed class BotMessage
{
    private readonly IBot _bot;
    private readonly uint? _groupId;

    public BotMessage(IBot bot, MessageChain message)
    {
        _bot = bot;
        QqMessage = message;
        SourceType = MessageSource.Qq;
    }

    public BotMessage(IBot bot, uint groupId)
    {
        _bot = bot;
        _groupId = groupId;
        SourceType = MessageSource.Qq;
    }

    public BotMessage(IBot bot, TgMessage message)
    {
        _bot = bot;
        TgMessage = message;
        SourceType = MessageSource.Telegram;
    }

    public MessageChain? QqMessage { get; }
    public TgMessage? TgMessage { get; }
    public MessageSource SourceType { get; }

    public string? Text => SourceType switch
    {
        MessageSource.Qq => QqMessage!.GetEntity<TextEntity>()?.Text,
        MessageSource.Telegram => TgMessage!.Text,
        _ => throw new ArgumentOutOfRangeException(nameof(SourceType), SourceType, null)
    };

    public long ChatId => _groupId ?? SourceType switch
    {
        MessageSource.Qq => QqMessage!.GroupUin!.Value,
        MessageSource.Telegram => TgMessage!.Chat.Id,
        _ => throw new ArgumentOutOfRangeException(nameof(SourceType), SourceType, null)
    };

    public async Task Reply(MessagePair messages, bool noReply = false)
    {
        await _bot.SendMessageAsync(messages, this, noReply);
    }
}