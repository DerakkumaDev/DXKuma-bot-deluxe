using Lagrange.Core.Message;
using TgMessage = Telegram.Bot.Types.Message;

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

    public string Text => SourceType switch
    {
        MessageSource.Qq => QqMessage?.ToPreviewText(),
        MessageSource.Telegram => TgMessage?.Text,
        _ => throw new ArgumentOutOfRangeException(nameof(SourceType), SourceType, null)
    } ?? throw new NullReferenceException();

    public long ChatId => _groupId ?? SourceType switch
    {
        MessageSource.Qq => QqMessage?.GroupUin,
        MessageSource.Telegram => TgMessage?.Chat.Id,
        _ => throw new ArgumentOutOfRangeException(nameof(SourceType), SourceType, null)
    } ?? throw new NullReferenceException();

    public async Task Reply(MessagePair messages, bool noReply = false)
    {
        await _bot.SendMessageAsync(messages, this, noReply);
    }
}