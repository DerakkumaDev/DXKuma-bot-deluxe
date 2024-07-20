using Lagrange.Core.Event.EventArg;
using TgMessage = Telegram.Bot.Types.Message;

namespace DXKumaBot.Bot;

public class MessageReceivedEventArgs : EventArgs
{
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
    
    private readonly IBot _bot;
    public GroupMessageEvent? QqMessage { get; }
    public TgMessage? TgMessage { get; }
}