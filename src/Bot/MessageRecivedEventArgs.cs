using DXKumaBot.Bot.Message;
using Lagrange.Core.Event.EventArg;
using TgMessage = Telegram.Bot.Types.Message;

namespace DXKumaBot.Bot;

public class MessageReceivedEventArgs : EventArgs
{
    private readonly IBot _bot;

    public MessageReceivedEventArgs(IBot bot, GroupMessageEvent message)
    {
        _bot = bot;
        QqMessage = message;
    }

    public MessageReceivedEventArgs(IBot bot, TgMessage message)
    {
        _bot = bot;
        TgMessage = message;
    }

    public GroupMessageEvent? QqMessage { get; }
    public TgMessage? TgMessage { get; }
    public string Text => QqMessage?.Chain.ToPreviewText() ?? TgMessage?.Text ?? throw new NullReferenceException();

    public async Task Reply(MessagePair messages, bool noReply = false)
    {
        await _bot.SendMessageAsync(this, messages,
            noReply ? default : QqMessage is null ? TgMessage ?? throw new NullReferenceException() : QqMessage);
    }
}